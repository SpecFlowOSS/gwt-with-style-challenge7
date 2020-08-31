using System.Collections.Generic;

namespace PaymentRequest.ISO20222.Data
{
    public class PaymentOrder
    {
        public List<PaymentTransaction> Transactions = new List<PaymentTransaction>();
    }
}