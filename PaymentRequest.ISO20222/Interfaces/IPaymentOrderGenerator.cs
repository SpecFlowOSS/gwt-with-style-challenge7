using PaymentRequest.ISO20222.Data;

namespace PaymentRequest.ISO20222.Interfaces
{
    public interface IPaymentOrderGenerator
    {
        PaymentOrder CreatePaymentOrder(string message);
    }
}