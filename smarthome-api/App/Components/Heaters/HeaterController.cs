using System.Collections.Generic;
using System.Linq;

namespace SmarthomeAPI.App.Components.Heaters
{
    public class HeaterController : IComponentController, IComponentControllerWithDetector
    {
        public string Identify()
        {
            return "heaters";
        }

        public ComponentContext GetContext() => new HeaterContext();

        public ComponentCommander GetCommander() => new HeaterCommander(new HeaterResourcePool(3));

        public IEnumerable<ICommand> GetCommands()
        {
            var list = new List<ICommand>
            {
                new HeaterGetTemperature(),
                new HeaterSetTemperature()
            };
            foreach (var detector in GetDetectors())
            {
                list = list.Concat(detector.GetDetectors()).ToList();
            }

            return list;
        }

        public T GetCommand<T>(string command) where T : class, ICommand =>
            GetCommands().First(c => c.Identify() == command) as T;

        public IEnumerable<ComponentDetector> GetDetectors() => new List<ComponentDetector>
        {
            new HeaterDetector()
        };
    }
}