using FuelConsumptionCentralMonitoringSystem.Helpers;
using FuelConsumptionCentralMonitoringSystem.Models;
using FuelConsumptionCentralMonitoringSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace FuelConsumptionCentralMonitoringSystem
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            
            await Run(100, 40, 1);

            Logger.Log("done!");
            Console.ReadLine();
        }

        private static async Task Run(int maxMessagesToBuffer, int producersCount, int consumersCount)
        {
            Logger.Log("*** STARTING EXECUTION ***");
            Logger.Log($"producers #: {producersCount}, buffer size: {maxMessagesToBuffer}, consumers #: {consumersCount}");


            var channel = Channel.CreateBounded<Message>(maxMessagesToBuffer);

            var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;

            List<UtilityVechile> vechiles = new List<UtilityVechile>();
           
            for(int i=1; i<= producersCount; i++)
            {
                vechiles.Add(new UtilityVechile(i,15));
            }

            var tasks = new List<Task>(StartConsumers(channel, consumersCount, cancellationToken))
            {
                ProduceAsync(channel, vechiles, producersCount, tokenSource)
            };

            await Task.WhenAll(tasks);
            Logger.Log("*** EXECUTION COMPLETE ***");

        }

        private static Task[] StartConsumers(Channel<Message> channel, int consumersCount, CancellationToken cancellationToken)
        {
            var consumerTasks = Enumerable.Range(1, consumersCount)
                .Select(i => new ConsumerService(channel.Reader, i).BeginConsumeAsync(cancellationToken))
                .ToArray();
            return consumerTasks;
        }

        private static async Task ProduceAsync(Channel<Message> channel,
            List<UtilityVechile> vehicles,
            int producersCount,
            CancellationTokenSource tokenSource)
        {

            Parallel.For(0, vehicles.Count,
                   index =>
                   {
                       _ = new ProducerService(channel.Writer, vehicles[index].VehicleId)
                        .PushMsg(vehicles[index]).ConfigureAwait(false);
                   });

            Logger.Log("done publishing, closing writer");
            channel.Writer.Complete();

            Logger.Log("waiting for consumer to complete...");
            await channel.Reader.Completion;

            Logger.Log("Consumers done processing, shutting down...");
            tokenSource.Cancel();
        }
    }
}
