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

    public class HeaterGetTemperature : ICommand
    {
        public string Identify()
        {
            return "heaterGetTemperature";
        }

        public async Task<CommandResult> Execute(Component component, object[] args = null)
        {
            return new CommandResult(10, component);
        }
    }

    public class HeaterSetTemperature : ICommand
    {
        public string Identify()
        {
            return "heaterSetTemperature";
        }

        public async Task<CommandResult> Execute(Component component, object[] args = null)
        {
            return new CommandResult(false, component);
        }
    }
}