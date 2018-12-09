using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using InTheHand.Net.Sockets;

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
            var client = new BluetoothClient();
            var devices = client.DiscoverDevicesInRange();
            var nameRegex = new Regex(@"comet\s*blue", RegexOptions.IgnoreCase);
            res.Data = devices.Where(d => nameRegex.IsMatch(d.DeviceName)).Select(d => new CometBlueHeater
            {
                Temperature = 0,
                BaseComponent = new BaseComponent
                {
                    Identifier = d.DeviceAddress.ToString("C"),
                    Name = d.DeviceName,
                    Vendor = new Vendor
                    {
                        Name = "Comet Blue"
                    }
                }
            }).ToList();

            return new Task<CommandResult>(() => res);
        }
    }
}