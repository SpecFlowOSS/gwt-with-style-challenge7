using System;
using PaymentRequest.ISO20222.Services;

namespace PaymentRequest.ISO20222.Specs.Fakes
{
    public class FakeTimeService : TimeService
    {
        private DateTime? _setCurrentTime;

        public override DateTime CurrentTime => _setCurrentTime ?? base.CurrentTime;

        public void SetCurrentTime(in DateTime time)
        {
            _setCurrentTime = time;
        }
    }
}
