using lab3.Models;
using Microsoft.EntityFrameworkCore;

namespace lab3.Data;


public class FleetDbContext : DbContext
    {
        public FleetDbContext(DbContextOptions<FleetDbContext> options) : base(options) { }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Truck> Trucks { get; set; }
        public DbSet<Van> Vans { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<TransportOrder> TransportOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehicle>()
                .HasDiscriminator<string>("VehicleType")
                .HasValue<Truck>("Truck")
                .HasValue<Van>("Van");
            
            modelBuilder.Entity<Truck>().HasData(
                new Truck { Id = 1, RegistrationNumber = "TRUCK001", MaxLoadKg = 25000, TrailerLength = 13.5, IsAvailable = true },
                new Truck { Id = 2, RegistrationNumber = "TRUCK002", MaxLoadKg = 18000, TrailerLength = 10.0, IsAvailable = true }
            );

            modelBuilder.Entity<Van>().HasData(
                new Van { Id = 3, RegistrationNumber = "VAN001", MaxLoadKg = 3500, CargoVolume = 12.0, IsAvailable = true },
                new Van { Id = 4, RegistrationNumber = "VAN002", MaxLoadKg = 2800, CargoVolume = 8.5, IsAvailable = true }
            );

            modelBuilder.Entity<Driver>().HasData(
                new Driver { Id = 1, Name = "Jan Kowalski", LicenseNumber = "DL001", IsAvailable = true },
                new Driver { Id = 2, Name = "Adam Nowak", LicenseNumber = "DL002", IsAvailable = true },
                new Driver { Id = 3, Name = "Piotr Wiśniewski", LicenseNumber = "DL003", IsAvailable = true }
            );
        }
}