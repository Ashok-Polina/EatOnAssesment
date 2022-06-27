using FuelConsumptionCentralMonitoringSystem.Models;
using System.Threading;
using System.Threading.Tasks;

namespace FuelConsumptionCentralMonitoringSystem.Interfaces
{
    public interface IProducer
    {
        public Task PublishAsync(Message message, CancellationToken cancellationToken = default);
    }
}
