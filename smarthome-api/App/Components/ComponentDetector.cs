using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmarthomeAPI.App.Components
{
    public abstract class ComponentDetector : IGroupCommand
    {
        public abstract List<IGroupCommand> GetDetectors();
        public abstract Task<CommandResult> Execute(object[] args = null);
        public abstract string Identify();
    }

    public class ComponentDetectionException : Exception
    {
        public ComponentDetectionException()
        {
        }

        public ComponentDetectionException(string message) : base(message)
        {
        }
    }
}