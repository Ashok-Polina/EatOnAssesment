using System;
using System.Collections.Generic;
using System.Text;

namespace FuelConsumptionCentralMonitoringSystem.Models
{
    /// <summary>
    /// Utility Vechile Class
    /// </summary>
    public partial class UtilityVechile
    {
        public int VehicleId { get; set; }

        public int FuelTankCapacity { get; set; }

        //public int Mileage { get; set; }

        public UtilityVechile(int id,int fuelTankCapacity)
        {
            this.VehicleId = id; 
            this.FuelTankCapacity = fuelTankCapacity;
            
        }
    }
}
