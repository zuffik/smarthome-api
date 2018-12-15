using Microsoft.EntityFrameworkCore;
using SmarthomeAPI.App.Components.Heaters.CometBlue;

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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<CometBlueHeater>();
            base.OnModelCreating(builder);
        }
    }
}