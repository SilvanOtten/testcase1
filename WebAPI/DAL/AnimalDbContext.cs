using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI
{
    public class AnimalDbContext : DbContext
    {
        public AnimalDbContext() { }

        public AnimalDbContext(DbContextOptions<AnimalDbContext> options) : base(options) { }

        public virtual DbSet<Animal> Animals { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = Environment.GetEnvironmentVariable("AnimalDb")
                       ?? @"Server=localhost,1433;Database=AnimalDb;User Id=sa;Password=Geheim_101";

                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Fluent API
        }

        internal void EnsureDataSeeded()
        {
            using (var context = new AnimalDbContext())
            {
                context.Database.EnsureCreated();

                var testAnimal = context.Animals.FirstOrDefault();
                if (testAnimal == null)
                {
                    context.Animals.Add(new Animal { Title = "Vos", Description = "Sluw Beest" });
                    context.Animals.Add(new Animal { Title = "Kameel", Description = "Bulterig" });
                    context.Animals.Add(new Animal { Title = "Kaneel", Description = "Specerij" });
                    context.Animals.Add(new Animal { Title = "Kat", Description = "Meow" });
                    context.Animals.Add(new Animal { Title = "Slak", Description = "Beetje glipperig maar best OK" });
                }
                context.SaveChanges();
            }
        }
    }
}