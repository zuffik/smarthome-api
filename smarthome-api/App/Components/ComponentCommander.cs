using System;
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

        public async Task<CommandResult> DoWhenReady(Component component, IComponentCommand componentCommand,
            object[] args = null)
        {
            while (_freeResources == 0)
            {
                Thread.Sleep(200);
            }

            _freeResources--;
            var result = await componentCommand.Execute(component, args);
            _freeResources++;
            return result;
        }

        public async Task<CommandResult> DoWhenReady(IGroupCommand componentCommand,
            object[] args = null)
        {
            while (_freeResources == 0)
            {
                Thread.Sleep(200);
            }

            _freeResources--;
            var result = await componentCommand.Execute(args);
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

        public async Task<CommandResult> ExecuteCommand(Component component, IComponentCommand command,
            object[] args = null)
        {
            if (_resourcePool == null)
            {
                return await command.Execute(component, args);
            }

            return await _resourcePool.DoWhenReady(component, command, args);
        }

        public async Task<CommandResult> ExecuteCommand(IGroupCommand command,
            object[] args = null)
        {
            if (_resourcePool == null)
            {
                return await command.Execute(args);
            }

            return await _resourcePool.DoWhenReady(command, args);
        }
    }

    public class CommandResult
    {
        public object Data { get; set; }

        public Component Component { get; }

        public CommandResult(object data, Component component)
        {
            Data = data;
            Component = component;
        }

        public CommandResult(object data)
        {
            Data = data;
        }

        public CommandResult()
        {
        }
    }

    public static class CheckArgs
    {
        public static bool AreNotNull(object[] args = null)
        {
            return args != null;
        }

        public static bool HaveExactlyLength(int count, object[] args = null)
        {
            return AreNotNull(args) && args?.Length == count;
        }

        public static WrongArgumentsException GetException(string command, string constraints)
        {
            return new WrongArgumentsException($"Arguments for {command} must {constraints}");
        }
    }

    public class WrongArgumentsException : Exception
    {
        public WrongArgumentsException()
        {
        }

        public WrongArgumentsException(string message) : base(message)
        {
        }
    }

    public interface ICommand : IIdentifiable
    {
    }

    public interface IGroupCommand : ICommand
    {
        Task<CommandResult> Execute(object[] args = null);
    }

    public interface IComponentCommand : ICommand
    {
        Task<CommandResult> Execute(Component component, object[] args = null);
    }
}