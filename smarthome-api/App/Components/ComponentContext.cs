using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using SmarthomeAPI.App.Components.Heaters;

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
            if (optionsBuilder.IsConfigured) return;
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseMySQL(new MySqlConnection(connectionString));
        }
    }
}