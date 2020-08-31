using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using ISO20222.Pain;
using PaymentRequest.ISO20222.Data;
using PaymentRequest.ISO20222.Interfaces;

namespace PaymentRequest.ISO20222.Services
{
    public class PaymentOrderGenerator : IPaymentOrderGenerator
    {
        private readonly ITimeService _timeService;

        public PaymentOrderGenerator(ITimeService timeService)
        {
            this._timeService = timeService;

        }

        public PaymentOrder CreatePaymentOrder(string message)
        {
            //TODO: error handling
            var document = DeserializeIsoDocument(message);

            return new PaymentOrder
            {
                Transactions = CreateTransactions(document)
            };
        }

        private static Document DeserializeIsoDocument(string message)
        {
            var serializer = new XmlSerializer(typeof(Document));
            using var reader = XmlReader.Create(new StringReader(message));
            return (Document) serializer.Deserialize(reader);
        }

        private List<PaymentTransaction> CreateTransactions(Document document)
        {
            var externalReferenceGenerator = new PaymentTransactionExternalReferenceGenerator(document.CstmrCdtTrfInitn.GrpHdr.MsgId);

            return document.CstmrCdtTrfInitn.PmtInf
                .SelectMany(pi => pi.CdtTrfTxInf)
                .Select(tx => CreateTransaction(externalReferenceGenerator, tx))
                .ToList();
        }

        private PaymentTransaction CreateTransaction(PaymentTransactionExternalReferenceGenerator paymentTransactionExternalReferenceGenerator,
            CreditTransferTransactionInformation10 transactionInfo)
        {
            return
                new PaymentTransaction
                {
                    Account = GetAccount(transactionInfo.CdtrAcct.Id),
                    Currency = GetCurrency(transactionInfo.Amt),
                    Credit = GetCredit(transactionInfo.Amt),
                    SettlementDate = _timeService.CurrentTime.Date,
                    ExternalReference = paymentTransactionExternalReferenceGenerator.GetNext()
                };
        }

        private string GetAccount(AccountIdentification4Choice cdtrAcctId)
        {
            switch (cdtrAcctId.Item)
            {
                case string iban:
                    return iban.Substring(4);
                default: throw new NotSupportedException("Not supported account identification");
            }
        }

        private string GetCurrency(AmountType3Choice transactionInfoAmt)
        {
            switch (transactionInfoAmt.Item)
            {
                case ActiveOrHistoricCurrencyAndAmount instdAmt:
                    return instdAmt.Ccy;
                default: throw new NotSupportedException("Not supported currency/amount info");
            }
        }

        private decimal GetCredit(AmountType3Choice transactionInfoAmt)
        {
            switch (transactionInfoAmt.Item)
            {
                case ActiveOrHistoricCurrencyAndAmount instdAmt:
                    return instdAmt.Value;
                default: throw new NotSupportedException("Not supported currency/amount info");
            }
        }
    }
}
