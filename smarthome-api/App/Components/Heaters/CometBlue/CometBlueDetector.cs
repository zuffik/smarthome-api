using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SmarthomeAPI.App.Components.Heaters.CometBlue
{
    public class CometBlueDetector : IGroupCommand
    {
        public string Identify()
        {
            return "heaterDetectCometBlue";
        }

        public Task<CommandResult> Execute(object[] args = null)
        {
            var res = new CommandResult
            {
                Data = new List<CometBlueHeater>()
            };
            var startInfo = new ProcessStartInfo
            {
                FileName = "cometblue",
                Arguments = "discover",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            using (var process = Process.Start(startInfo))
            {
                using (var reader = process?.StandardOutput)
                {
                    var stderr = process?.StandardError.ReadToEnd();
                    var result = reader?.ReadToEnd();
                    if (stderr != "")
                    {
                        throw new ComponentDetectionException(stderr);
                    }

                    var regex = new Regex(
                        @"\(([0-9A-F]{2}:[0-9A-F]{2}:[0-9A-F]{2}:[0-9A-F]{2}:[0-9A-F]{2}:[0-9A-F]{2})\)",
                        RegexOptions.IgnoreCase);
                    var matches = regex.Matches(result);
                    foreach (Match match in matches)
                    {
                        var mac = match.Groups[0].Value;
                        var heater = new CometBlueHeater
                        {
                            Temperature = 0,
                            BaseComponent = new BaseComponent
                            {
                                Identifier = mac,
                                Name = mac,
                                Vendor = new Vendor
                                {
                                    Name = "Comet Blue"
                                }
                            }
                        };
                        ((List<CometBlueHeater>) res.Data).Add(heater);
                    }
                }
            }

            return new Task<CommandResult>(() => res);
        }
    }
}