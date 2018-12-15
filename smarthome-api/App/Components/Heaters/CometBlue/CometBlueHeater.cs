using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SmarthomeAPI.App.Components.Heaters.CometBlue
{
    public class CometBlueHeater : Heater, IHasPinCode
    {
        public override async Task<bool> SetTemperature(double temperature)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "cometblue",
                Arguments = $"device -p {GetPin()} {BaseComponent.Identifier} set temperatures {temperature}",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            using (var process = Process.Start(startInfo))
            {
                var stderr = process?.StandardError.ReadToEnd();
                if (stderr != "")
                {
                    throw new ComponentDetectionException(stderr);
                }

                process.WaitForExit();
                return process.ExitCode == 0;
            }
        }

        public override async Task<double> GetTemperature()
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "cometblue",
                Arguments = $"device -p {GetPin()} {BaseComponent.Identifier} get temperatures",
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
                        @"Current temperature: (\d*\.\d*) °C",
                        RegexOptions.IgnoreCase);
                    var m = regex.Match(result);
                    return double.Parse(m.Value);
                }
            }
        }

        public string GetPin()
        {
            return "112111";
        }
    }
}