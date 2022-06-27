using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using FuelConsumptionCentralMonitoringSystem.Helpers;
using FuelConsumptionCentralMonitoringSystem.Interfaces;
using FuelConsumptionCentralMonitoringSystem.Models;

namespace FuelConsumptionCentralMonitoringSystem.Services
{
    public class ProducerService : IProducer
    {

        private readonly ChannelWriter<Message> _writer;
        private readonly int _instanceId;


        public ProducerService(ChannelWriter<Message> writer, int instanceId)
        {
            _writer = writer;
            _instanceId = instanceId;
        }

        public async Task PushMsg(UtilityVechile vechile, CancellationTokenSource token)
        {
            try
            {
                while (vechile.FuelTankCapacity > 3)
                {
                    vechile.FuelTankCapacity = vechile.FuelTankCapacity - 1;
                    await PublishAsync(new Message() { TruckId = vechile.VehicleId, CurrentGas = vechile.FuelTankCapacity });

                }
               token.Cancel();
            }
            catch (OperationCanceledException ex)
            {
                _writer.Complete();
            }
        }

        public async Task PublishAsync(Message message, CancellationToken cancellationToken = default)
        {
            await _writer.WriteAsync(message, cancellationToken);
            Logger.Log($"Message Published: Vehicle ID: {message.TruckId}, Current Fuel: {message.CurrentGas} Gallons", ConsoleColor.Yellow);
        }
    }
}
