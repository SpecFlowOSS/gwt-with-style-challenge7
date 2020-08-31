using BoDi;
using PaymentRequest.ISO20222.Interfaces;
using PaymentRequest.ISO20222.Services;
using PaymentRequest.ISO20222.Specs.Fakes;
using TechTalk.SpecFlow;

namespace PaymentRequest.ISO20222.Specs.Hooks
{
    [Binding]
    public class ContainerSetup
    {
        private readonly IObjectContainer _objectContainer;

        public ContainerSetup(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        [BeforeScenario()]
        public void Initialize()
        {
            _objectContainer.RegisterTypeAs<FakeTimeService, ITimeService>();
            _objectContainer.RegisterTypeAs<PaymentOrderGenerator, IPaymentOrderGenerator>();
        }
    }
}
