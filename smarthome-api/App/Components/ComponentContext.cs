using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using SmarthomeAPI.App.Components.Heaters;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace SmarthomeAPI.App.Components
{
    public abstract class ComponentContext : DbContext
    {
        public DbSet<Component> Components { get; set; }

        protected ComponentContext()
        {
            Database.EnsureCreated();
        }

        protected ComponentContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseMySQL(new MySqlConnection(connectionString));
            }
        }
    }

    public interface IIdentifiable
    {
        string Identify();
        Type GetType();
    }

    public interface IComponentController : IIdentifiable
    {
        ComponentContext GetContext();
        ComponentCommander GetCommander();
        List<ICommand> GetCommands();
    }

    public class ComponentControllers
    {
        public List<IComponentController> Controllers { get; set; } = new List<IComponentController>();

        private static ComponentControllers _inst;

        public static ComponentControllers Instance => _inst = _inst ?? new ComponentControllers();

        private ComponentControllers()
        {
            Controllers.Add(new HeaterController());
        }

        public IComponentController GetController(string name)
        {
            return Controllers.First(c => c.Identify() == name);
        }
    }
}