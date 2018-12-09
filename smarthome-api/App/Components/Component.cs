using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace SmarthomeAPI.App.Components
{
    public class Vendor
    {
        [Key] public int Id { get; set; }
        public string Name { get; set; }
    }

    public class BaseComponent
    {
        [Key] public int Id { get; set; }
        [Index(IsUnique = true)] public string Identifier { get; set; }
        public string Name { get; set; }
        public int VendorId { get; set; }
        public Vendor Vendor { get; set; }
    }

    public abstract class Component
    {
        [Key] public int Id { get; set; }
        public int BaseComponentId { get; set; }
        public BaseComponent BaseComponent { get; set; }
    }

    public interface IHasTemperature
    {
        Task<bool> SetTemperature(double temperature);
        Task<double> GetTemperature();
    }

    public interface IHasPinCode
    {
        string GetPin();
    }
}