using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using FuelConsumptionCentralMonitoringSystem.Helpers;
using FuelConsumptionCentralMonitoringSystem.Interfaces;
using FuelConsumptionCentralMonitoringSystem.Models;

namespace FuelConsumptionCentralMonitoringSystem.Services
{
    public class ConsumerService : IConsumer
    {
        private readonly ChannelReader<Message> _reader;
        private readonly int _instanceId;
        private static readonly Random Random = new Random();

        public Dictionary<int, int> fuelDictionary = new Dictionary<int, int>();

        public ConsumerService(ChannelReader<Message> reader, int instanceId)
        {
            _reader = reader;
            _instanceId = instanceId;
        }

        public async Task BeginConsumeAsync(CancellationToken cancellationToken = default)
        {
            Logger.Log($"Consumer {_instanceId} > starting", ConsoleColor.Green);

            try
            {
                await foreach (var message in _reader.ReadAllAsync(cancellationToken))
                {
                    fuelDictionary[message.TruckId] = message.CurrentGas;
                    Logger.Log($"Message Received: Vehicle ID: {message.TruckId}, Current Fuel: {message.CurrentGas} Gallons, FuelNeeded: {15 - message.CurrentGas} Gallons  ", ConsoleColor.Green);
                    if (message.CurrentGas <= 3)
                    {
                        CMSService.DisplayReport(fuelDictionary);
                        await _reader.Completion;
                        Logger.Log("Reading Completed...");
                    }

                }

                
            }
            catch (OperationCanceledException ex)
            {
                Logger.Log($"Consumer {_instanceId} > forced stop", ConsoleColor.DarkRed);
            }

            Logger.Log($"Consumer {_instanceId} > shutting down", ConsoleColor.DarkRed);
        }

    }
}
