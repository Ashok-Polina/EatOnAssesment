using System;
using System.Collections.Generic;
using System.Text;

namespace FuelConsumptionCentralMonitoringSystem.Models
{
    /// <summary>
    /// Refuelling Vechile
    /// </summary>
    public class RefuelingTruck
    {
        /// <summary>
        /// Truck Id
        /// </summary>
        public int TruckId { get; set; }

        public int FuelTankCapacity { get; set; }

    }
}
