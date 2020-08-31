using System;

namespace PaymentRequest.ISO20222.Data
{
    public class PaymentTransaction
    {
        public string Account { get; set; }
        public string Currency { get; set; }
        public decimal Credit { get; set; }
        
        public DateTime SettlementDate { get; set; }
        public string ExternalReference { get; set; }

        public override string ToString() =>
            $"PT [Account:{Account},Currency:{Currency},Credit:{Credit},SettlementDate:{SettlementDate},ExternalReference:{ExternalReference}]";
    }
}