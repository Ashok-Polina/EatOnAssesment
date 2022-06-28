using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using FuelConsumptionCentralMonitoringSystem.Helpers;
using FuelConsumptionCentralMonitoringSystem.Interfaces;
using FuelConsumptionCentralMonitoringSystem.Models;

namespace FuelConsumptionCentralMonitoringSystem.Services
{
    /// <summary>
    /// Service to post messages to Channel
    /// </summary>
    public class ProducerService : IProducer
    {

        private readonly ChannelWriter<Message> _writer;
        private readonly int _instanceId;

        public ProducerService(ChannelWriter<Message> writer, int instanceId)
        {
            _writer = writer;
            _instanceId = instanceId;
        }

        public async Task PushMsg(UtilityVechile vechile, CancellationTokenSource source)
        {
            try
            {
                
                while (vechile.FuelTankCapacity > 3)
                {
                    vechile.FuelTankCapacity = vechile.FuelTankCapacity - 1;
                    await PublishAsync(new Message() { TruckId = vechile.VehicleId, CurrentGas = vechile.FuelTankCapacity }, source.Token);
                }
                source.Cancel();
                _writer.TryComplete();
                
            }
            catch (ChannelClosedException e)
            {
                Console.WriteLine("Channel was closed!");
            }
            catch (OperationCanceledException ex)
            {
                _writer.Complete();
            }
            finally
            {
                source.Dispose();
            }
        }

        /// <summary>
        /// Posts Message to channel
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task PublishAsync(Message message, CancellationToken cancellationToken = default)
        {

            if (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await _writer.WriteAsync(message, cancellationToken);
                    Logger.Log($"Message Published: Vehicle ID: {message.TruckId}, Current Fuel: {message.CurrentGas} Gallons", ConsoleColor.Yellow);

                }
                catch (ChannelClosedException e)
                {
                    Console.WriteLine("Channel was closed!");
                }

            }
             
        }
    }
}
