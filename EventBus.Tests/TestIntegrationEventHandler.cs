using MedicShop.EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Tests
{
    public class TestIntegrationEventHandler : IIntegrationEventHandler<TestIntegrationEvent>
    {
        public TestIntegrationEventHandler()
        {
            Handled = false;
        }

        public bool Handled { get; private set; }

        public async Task Handle(TestIntegrationEvent @event)
        {
            Handled = true;
        }
    }
}
