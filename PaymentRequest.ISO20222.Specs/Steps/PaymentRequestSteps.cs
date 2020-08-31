using FluentAssertions;
using PaymentRequest.ISO20222.Data;
using PaymentRequest.ISO20222.Specs.Drivers;
using PaymentRequest.ISO20222.Specs.Support;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace PaymentRequest.ISO20222.Specs.Steps
{
    [Binding]
    public class PaymentRequestSteps
    {
        private readonly MessageTemplatesRegistry _messageTemplatesRegistry;
        private readonly PaymentOrderGeneratorDriver _paymentOrderGeneratorDriver;
        private readonly MessageBuilder _messageBuilder;

        public PaymentRequestSteps(MessageTemplatesRegistry messageTemplatesRegistry, PaymentOrderGeneratorDriver paymentOrderGeneratorDriver, MessageBuilder messageBuilder)
        {
            _messageTemplatesRegistry = messageTemplatesRegistry;
            _paymentOrderGeneratorDriver = paymentOrderGeneratorDriver;
            _messageBuilder = messageBuilder;
        }

        [Given(@"the following message templates")]
        public void GivenTheFollowingMessageTemplates(Table table)
        {
            foreach (var row in table.Rows)
            {
                _messageTemplatesRegistry.Register(row["template"], row["file"]);
            }
        }

        [Given(@"the ""(.*)"" message with the following header")]
        public void GivenTheMessageWithTheFollowingHeader(string templateName, Table header)
        {
            _messageBuilder.CreateMessage(templateName);
            
            _messageBuilder.SetupNode(
                new DotPath("CstmrCdtTrfInitn.GrpHdr"), 
                header.AsSingleRow());
        }

        [Given(@"the message contains a Payment Information block with the following header")]
        public void GivenTheMessageContainsAPaymentInformationBlockWithTheFollowingHeader(Table header)
        {
            _messageBuilder.SetupNode(
                new DotPath("CstmrCdtTrfInitn.PmtInf"), 
                header.AsSingleRow());
        }

        [Given(@"the Payment Information block contains the following transfers")]
        public void GivenThePaymentInformationBlockContainsTheFollowingTransfers(Table transfers)
        {
            _messageBuilder.SetupNodes(
                new DotPath("CstmrCdtTrfInitn.PmtInf.CdtTrfTxInf"), 
                transfers.AsAllRows());
        }

        [When(@"the payment order is generated for the message")]
        public void WhenThePaymentOrderIsGeneratedForTheMessage()
        {
            var message = _messageBuilder.GetMessage();
            _paymentOrderGeneratorDriver.GeneratePaymentOrder(message);
        }

        [Then(@"the payment order contains the following transactions")]
        public void ThenThePaymentOrderContainsTheFollowingTransactions(Table table)
        {
            var expectedTransactions = table.CreateSet<PaymentTransaction>();
            var actualTransactions = _paymentOrderGeneratorDriver.GeneratedPaymentOrder?.Transactions;

            actualTransactions.Should().BeEquivalentTo(expectedTransactions);
        }
    }
}
