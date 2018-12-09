using System.Threading;
using System.Threading.Tasks;

namespace SmarthomeAPI.App.Components
{
    public abstract class ComponentResourcePool
    {
        private int MaxResources { get; }
        private int _freeResources;

        protected ComponentResourcePool(int maxResources)
        {
            MaxResources = _freeResources = maxResources;
        }

        public async Task<CommandResult> DoWhenReady(Component component, ICommand command, object[] args = null)
        {
            while (_freeResources == 0)
            {
                Thread.Sleep(200);
            }

            _freeResources--;
            var result = await command.Execute(component, args);
            _freeResources++;
            return result;
        }
    }

    public abstract class ComponentCommander
    {
        private readonly ComponentResourcePool _resourcePool;

        protected ComponentCommander(ComponentResourcePool resourcePool = null)
        {
            _resourcePool = resourcePool;
        }

        public async Task<CommandResult> ExecuteCommand(Component component, ICommand command, object[] args = null)
        {
            if (_resourcePool == null)
            {
                return await command.Execute(component, args);
            }

            return await _resourcePool.DoWhenReady(component, command, args);
        }
    }

    public class CommandResult
    {
        private object Data { get; }

        private Component Component { get; }

        public CommandResult(object data, Component component)
        {
            Data = data;
            Component = component;
        }
    }

    public interface ICommand : IIdentifiable
    {
        Task<CommandResult> Execute(Component component, object[] args = null);
    }
}