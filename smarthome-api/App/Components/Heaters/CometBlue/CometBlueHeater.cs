using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

// TODO: resource: https://github.com/im-0/cometblue (possible rewrite to c#)
namespace SmarthomeAPI.App.Components.Heaters.CometBlue
{
    public class CometBlueHeater : Heater, IHasPinCode
    {
        public override Task<bool> SetTemperature(double temperature)
        {
            throw new NotImplementedException();
        }

        public override Task<double> GetTemperature()
        {
            throw new NotImplementedException();
        }

        public string GetPin()
        {
            return "112111";
        }
    }
}