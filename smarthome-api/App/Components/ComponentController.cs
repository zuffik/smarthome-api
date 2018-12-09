using System;
using System.Collections.Generic;
using System.Linq;
using SmarthomeAPI.App.Components.Heaters;

namespace SmarthomeAPI.App.Components
{
    public interface IIdentifiable
    {
        string Identify();
        Type GetType();
    }

    public interface IComponentController : IIdentifiable
    {
        ComponentContext GetContext();
        ComponentCommander GetCommander();
        IEnumerable<ICommand> GetCommands();
        T GetCommand<T>(string command) where T : class, ICommand;
    }

    public interface IComponentControllerWithDetector
    {
        IEnumerable<ComponentDetector> GetDetectors();
    }

    public class ComponentControllers
    {
        public List<IComponentController> Controllers { get; set; } = new List<IComponentController>();

        private static ComponentControllers _inst;

        public static ComponentControllers Instance => _inst = _inst ?? new ComponentControllers();

        private ComponentControllers()
        {
            Controllers.Add(new HeaterController());
        }

        public IComponentController GetController(string name)
        {
            return Controllers.First(c => c.Identify() == name);
        }
    }
}