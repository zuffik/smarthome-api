using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace SmarthomeAPI.App.Components.Heaters
{
    public class HeaterContext : ComponentContext
    {
        public new DbSet<Heater> Components { get; set; }

        public HeaterContext()
        {
        }

        public HeaterContext(DbContextOptions options) : base(options)
        {
        }
    }
}