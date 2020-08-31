using System;
using PaymentRequest.ISO20222.Interfaces;

namespace PaymentRequest.ISO20222.Services
{
    public class TimeService : ITimeService
    {
        public virtual DateTime CurrentTime => DateTime.Now;
    }
}