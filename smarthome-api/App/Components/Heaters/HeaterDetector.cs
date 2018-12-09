using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarthomeAPI.App.Components.Heaters
{
    public class HeaterDetector : ComponentDetector
    {
        public override List<IGroupCommand> GetDetectors()
        {
            return new List<IGroupCommand>
            {
                new CometBlueDetector()
            };
        }

        public override async Task<CommandResult> Execute(object[] args = null)
        {
            var result = new CommandResult
            {
                Data = new List<Heater>()
            };
            foreach (var detector in GetDetectors())
            {
                result.Data = ((List<Heater>) result.Data).Concat((await detector.Execute(args)).Data as List<Heater> ??
                                                    throw new NullReferenceException());
            }

            return result;
        }
    }

    public class CometBlueDetector : IGroupCommand
    {
        public string Identify()
        {
            return "heaterDetectCometBlue";
        }

        public Task<CommandResult> Execute(object[] args = null)
        {
            throw new NotImplementedException();
        }
    }
}