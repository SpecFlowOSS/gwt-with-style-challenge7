namespace PaymentRequest.ISO20222.Services
{
    public class PaymentTransactionExternalReferenceGenerator
    {
        private readonly string _msgId;
        private int _counter = 0;

        public PaymentTransactionExternalReferenceGenerator(string msgId)
        {
            _msgId = msgId;
        }

        public string GetNext() => $"{_msgId}-{++_counter}";
    }
}