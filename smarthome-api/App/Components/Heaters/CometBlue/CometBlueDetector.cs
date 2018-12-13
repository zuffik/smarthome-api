using System;
using System.Linq;
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
            try
            {
                return new Task<CommandResult>(() => new CommandResult
                {
                    Data = new BluetoothClient().DiscoverDevices(5).Select(device => new CometBlueHeater
                    {
                        BaseComponent =
                        {
                            Identifier = device.DeviceAddress.ToString("C"),
                            Name = device.DeviceName,
                            Vendor =
                            {
                                Id = (int) Vendors.HEATER_COMET_BLUE,
                                Name = "Comet Blue"
                            },
                            VendorId = (int) Vendors.HEATER_COMET_BLUE
                        }
                    })
                });
            }
            catch (PlatformNotSupportedException e)
            {
                throw new ComponentDetectionException(
                    $"Unsupported platform or bluetooth client not found. (Reason: '${e.Message}')");
            }
        }
    }
}