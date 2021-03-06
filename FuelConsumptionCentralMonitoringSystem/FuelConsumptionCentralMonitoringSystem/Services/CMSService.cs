using System.Collections.Generic;
using System.Linq;
using System;
using FuelConsumptionCentralMonitoringSystem.Helpers;

namespace FuelConsumptionCentralMonitoringSystem.Services
{

    public class TruckData
    {
        public List<int> utilityTrucksId { get; set; }
        public int requiredFuel { get; set; }

        public TruckData()
        {
            utilityTrucksId = new List<int>();
        }
    }
    /// <summary>
    /// Central Monitoring System
    /// </summary>
    public static class CMSService
    {
        /// <summary>
        /// To Display Consumption and Required Fuel Reports
        /// </summary>
        /// <param name="fuelRemained"></param>
        public static void DisplayReport(Dictionary<int, int> fuelRemained)
        {
           
            Dictionary<int, TruckData> refuelTruck =  CreateRefuelTruckData(fuelRemained);

            foreach (var truck in refuelTruck.Keys)
            {
                if (refuelTruck[truck].requiredFuel == 0)
                {
                    break;
                }
                Logger.Log($"\n\nFuel Truck Id {truck} is going out with {refuelTruck[truck].requiredFuel + 5} gallons of fuel for the following vehicles.", ConsoleColor.Cyan);

                foreach (var vehicleId in refuelTruck[truck].utilityTrucksId)
                {
                    Logger.Log($" \t Vehicle Id {vehicleId}, Current Fuel : {fuelRemained[vehicleId]} gallons, Fuel Needed : {15 - fuelRemained[vehicleId]} gallons", ConsoleColor.Magenta);
                }
            }

        
        }

        /// <summary>
        /// To Calculate Required Refuelling Trucks and Required Fuel
        /// </summary>
        /// <param name="fuelConsumption"></param>
        /// <returns></returns>
        private static Dictionary<int, TruckData> CreateRefuelTruckData(Dictionary<int, int> fuelConsumption)
        {
            var fuelConsumptionList = fuelConsumption.OrderBy(key => key.Value).ToList();

            Dictionary<int, TruckData> refuelTruck = new Dictionary<int, TruckData>();

            int totalfuelRemained = fuelConsumption.Sum(fuel => fuel.Value);
            int requiredFuel = fuelConsumption.Count * 15 - totalfuelRemained;
            int requiredTrucks = requiredFuel / 35  + 1;
            
            int i = 0;
            int j = fuelConsumptionList.Count - 1;

            for (int t = 1; t <= requiredTrucks; t++)
            {
                int currentRequiredFuel = 0;
                refuelTruck.Add(t, new TruckData());
                bool flip = false;
                while (i <= j)
                {
                    if (flip)
                    {
                        if (currentRequiredFuel + (15 - fuelConsumptionList[j].Value) > 35)
                        {
                            break;
                        }
                        currentRequiredFuel = currentRequiredFuel + (15 - fuelConsumptionList[j].Value);
                        refuelTruck[t].utilityTrucksId.Add(fuelConsumptionList[j].Key);
                        j--;
                    }
                    else
                    {
                        if (currentRequiredFuel + (15 - fuelConsumptionList[i].Value) > 35)
                        {
                            break;
                        }
                        currentRequiredFuel = currentRequiredFuel + (15 - fuelConsumptionList[i].Value);
                        refuelTruck[t].utilityTrucksId.Add(fuelConsumptionList[i].Key);
                        i++;
                    }
                    flip = !flip;

                }
                if (currentRequiredFuel == 0)
                {
                    break;
                }
                refuelTruck[t].requiredFuel = currentRequiredFuel;

            }

            return refuelTruck;
        }
    }
}
