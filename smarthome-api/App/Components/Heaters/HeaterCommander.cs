using System;
using System.Threading.Tasks;

namespace SmarthomeAPI.App.Components.Heaters
{
    public class HeaterResourcePool : ComponentResourcePool
    {
        public HeaterResourcePool(int maxResources) : base(maxResources)
        {
        }
    }

    public class HeaterCommander : ComponentCommander
    {
        public HeaterCommander(ComponentResourcePool resourcePool = null) : base(resourcePool)
        {
        }
    }

    public class HeaterGetTemperature : IComponentCommand
    {
        public string Identify()
        {
            return "heaterGetTemperature";
        }

        public Task<CommandResult> Execute(Component component, object[] args = null)
        {
            var temperature = ((Heater) component).GetTemperature();
            return new Task<CommandResult>(() => new CommandResult(temperature, component));
        }
    }

    public class HeaterSetTemperature : IComponentCommand
    {
        public string Identify()
        {
            return "heaterSetTemperature";
        }

        public Task<CommandResult> Execute(Component component, object[] args = null)
        {
            if (!CheckArgs.HaveExactlyLength(1, args))
            {
                throw CheckArgs.GetException(Identify(), "have at least 1 argument");
            }

            var didSet = ((Heater) component).SetTemperature(Convert.ToDouble(args?[0]));
            return new Task<CommandResult>(() => new CommandResult(didSet, component));
        }
    }
}