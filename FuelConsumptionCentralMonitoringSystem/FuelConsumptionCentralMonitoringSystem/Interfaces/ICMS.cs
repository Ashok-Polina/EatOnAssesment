using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FuelConsumptionCentralMonitoringSystem.Interfaces
{
    public interface ICMS
    {
        public  Task DisplayReport(Dictionary<int, int> fuelRemained);
    }
}
