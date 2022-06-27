using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FuelConsumptionCentralMonitoringSystem.Interfaces
{
    public interface IConsumer
    {
        public Task BeginConsumeAsync(CancellationToken cancellationToken = default);
    }
}
