using PaymentRequest.ISO20222.Data;
using PaymentRequest.ISO20222.Interfaces;

namespace PaymentRequest.ISO20222.Specs.Drivers
{
    public class PaymentOrderGeneratorDriver
    {
        private readonly IPaymentOrderGenerator _generator;

        public PaymentOrderGeneratorDriver(IPaymentOrderGenerator generator)
        {
            _generator = generator;
        }

        public void GeneratePaymentOrder(string message)
        {
            GeneratedPaymentOrder = _generator.CreatePaymentOrder(message);
        }

        public PaymentOrder GeneratedPaymentOrder { get; private set; }
    }
}
