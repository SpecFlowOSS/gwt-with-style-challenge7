using System;
using PaymentRequest.ISO20222.Specs.Fakes;
using TechTalk.SpecFlow;

namespace PaymentRequest.ISO20222.Specs.Steps
{
    [Binding]
    public class TimeSteps
    {
        private readonly FakeTimeService _fakeTimeService;

        public TimeSteps(FakeTimeService fakeTimeService)
        {
            _fakeTimeService = fakeTimeService;
        }

        [Given(@"the current time is (.*)")]
        public void GivenTheCurrentTimeIs(DateTime time)
        {
            _fakeTimeService.SetCurrentTime(time);
        }
    }
}
