using System;

namespace PaymentRequest.ISO20222.Interfaces
{
    public interface ITimeService
    {
        public DateTime CurrentTime { get; }
    }
}
