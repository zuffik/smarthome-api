using System;

namespace SmarthomeAPI.App.Components.Heaters
{
    public class HeaterDto : Component
    {
        public int Temperature { get; set; }
    }
    
    public abstract class Heater : HeaterDto
    {
        public abstract bool SetTemperature(double temperature);

        public abstract double GetTemperature();
    }

    public class CometBlue : Heater
    {
        public override bool SetTemperature(double temperature)
        {
            throw new NotImplementedException();
        }

        public override double GetTemperature()
        {
            throw new NotImplementedException();
        }
    }
}