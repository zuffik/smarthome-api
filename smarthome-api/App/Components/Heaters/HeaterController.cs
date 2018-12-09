using System.Collections.Generic;

namespace SmarthomeAPI.App.Components.Heaters
{
    public class HeaterController : IComponentController
    {
        public string Identify()
        {
            return "heaters";
        }

        public ComponentContext GetContext()
        {
            return new HeaterContext();
        }

        public ComponentCommander GetCommander()
        {
            return new HeaterCommander(new HeaterResourcePool(3));
        }

        public List<ICommand> GetCommands()
        {
            return new List<ICommand>
            {
                new HeaterGetTemperature(),
                new HeaterSetTemperature()
            };
        }
    }
}