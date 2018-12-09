using System;
using System.Threading.Tasks;

namespace SmarthomeAPI.App.Components.Heaters
{
    public class HeaterDto : Component
    {
        public int Temperature { get; set; }
    }
    
    public abstract class Heater : HeaterDto, IHasTemperature
    {
        public abstract Task<bool> SetTemperature(double temperature);
        public abstract Task<double> GetTemperature();
    }
}