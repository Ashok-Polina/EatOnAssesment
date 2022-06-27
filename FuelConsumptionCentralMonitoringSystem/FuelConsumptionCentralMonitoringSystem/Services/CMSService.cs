using System.Collections.Generic;
using System.Linq;
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
    public static class CMSService
    {
        public static void DisplayReport(Dictionary<int, int> fuelRemained)
        {
           
            Dictionary<int, TruckData> refuelTruck =  CreateRefuelTruckData(fuelRemained);

            foreach (var truck in refuelTruck.Keys)
            {
                if (refuelTruck[truck].requiredFuel == 0)
                {
                    break;
                }
                Logger.Log($"Fuel Truck Id {truck} is going out with {refuelTruck[truck].requiredFuel + 5} gallons of fuel for the following vehicles.");

                foreach (var vehicleId in refuelTruck[truck].utilityTrucksId)
                {
                    Logger.Log($" \t Vehicle Id {vehicleId}, Current Fuel : {fuelRemained[vehicleId]} gallons, Fuel Needed : {15 - fuelRemained[vehicleId]} gallons");
                }
            }

        
        }

        private static Dictionary<int, TruckData> CreateRefuelTruckData(Dictionary<int, int> fuelConsumption)
        {
            var fuelConsumptionList = fuelConsumption.OrderBy(key => key.Value).ToList();

            Dictionary<int, TruckData> refuelTruck = new Dictionary<int, TruckData>();

            int totalfuelConsumed = fuelConsumption.Sum(fuel => fuel.Value);
            int requiredFuel = fuelConsumption.Count * 15 - totalfuelConsumed;
            int requiredTrucks = totalfuelConsumed/ 35;
            if(totalfuelConsumed % 35 != 0)
            {
                requiredTrucks++;
            }
            bool flip = false;
            int i = 0;
            int j = fuelConsumptionList.Count - 1;

            for (int t = 1; t <= requiredTrucks; t++)
            {
                int currentRequiredFuel = 0;
                refuelTruck.Add(t, new TruckData());
                while (i < j)
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
