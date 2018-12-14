using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

        public async Task<CommandResult> Execute(object[] args = null)
        {
            // timeout 15s hcitool lescan
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = "timeout",
                    Arguments = "15s hcitool lescan",
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };
                using (var process = Process.Start(startInfo))
                {
                    using (var reader = process?.StandardOutput)
                    {
                        var stderr = process?.StandardError.ReadToEnd();
                        var result = reader?.ReadToEnd();
                        if (string.IsNullOrEmpty(stderr))
                        {
                            throw new ComponentDetectionException(
                                $"Error happened during discovery. (Reason: '{stderr}')");
                        }

                        var lines = result.Split("\n").Skip(1);
                        var deviceInfo = new Regex(
                            @"(?<mac>[0-9A-F]{2}:[0-9A-F]{2}:[0-9A-F]{2}:[0-9A-F]{2}:[0-9A-F]{2}:[0-9A-F]{2})\s*(?<name>.*)$",
                            RegexOptions.IgnoreCase
                        );
                        var isVendor = new Regex(@"Comet\s*Blue", RegexOptions.IgnoreCase);
                        return new CommandResult
                        {
                            Data = lines.Where(l => isVendor.IsMatch(l)).Select(line =>
                            {
                                var match = deviceInfo.Match(line);
                                return new CometBlueHeater
                                {
                                    BaseComponent = new BaseComponent
                                    {
                                        Identifier = match.Groups["mac"].Value,
                                        Name = match.Groups["name"].Value,
                                        Vendor = new Vendor
                                        {
                                            Id = (int) Vendors.HEATER_COMET_BLUE,
                                            Name = "Comet Blue"
                                        },
                                        VendorId = (int) Vendors.HEATER_COMET_BLUE
                                    }
                                };
                            })
                        };
                    }
                }
            }
            catch (NullReferenceException e)
            {
                throw new ComponentDetectionException(
                    $"Error happened during discovery. (Reason: '{e.Message}')");
            }
            catch (PlatformNotSupportedException e)
            {
                throw new ComponentDetectionException(
                    $"Unsupported platform or bluetooth client not found. (Reason: '{e.Message}')");
            }
        }
    }
}