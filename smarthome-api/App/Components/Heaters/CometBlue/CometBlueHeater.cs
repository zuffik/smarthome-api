using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SmarthomeAPI.App.Components.Heaters.CometBlue
{
    public class CometBlueHeater : Heater, IHasPinCode
    {
        public override Task<bool> SetTemperature(double temperature)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "cometblue",
                Arguments = "discover",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                ArgumentList =
                {
                    "-p ",
                    GetPin(),
                    BaseComponent.Identifier,
                    "get temperatures"
                }
            };
            using (var process = Process.Start(startInfo))
            {
                using (var reader = process?.StandardOutput)
                {
                    var stderr = process?.StandardError.ReadToEnd();
                    if (stderr != "")
                    {
                        throw new ComponentDetectionException(stderr);
                    }

                    process.WaitForExit();
                    var ec = process.ExitCode;
                    return new Task<bool>(() => ec == 0);
                }
            }
        }

        public override Task<double> GetTemperature()
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "cometblue",
                Arguments = "discover",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                ArgumentList =
                {
                    "-p ",
                    GetPin(),
                    BaseComponent.Identifier,
                    "get temperatures"
                }
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
                        @"Current temperature: (\d*\.\d*) °C",
                        RegexOptions.IgnoreCase);
                    var m = regex.Match(result);
                    return new Task<double>(() => double.Parse(m.Value));
                }
            }
        }

        public string GetPin()
        {
            return "112111";
        }
    }
}