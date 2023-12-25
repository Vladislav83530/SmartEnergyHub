using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartEnergyHub.DAL.Entities;
using SmartEnergyHub.DAL.Entities.APIUser;

namespace SmartEnergyHub.DAL.EF
{
    public class ApplicationDbContext : IdentityDbContext<Customer>
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Residence> Residences { get; set; }
        public DbSet<ResidenceLocation> ResidenceLocations { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<DeviceInfo> DeviceInfo { get; set; }
        public DbSet<ActivitySession> ActivitySessions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public ApplicationDbContext() { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<Residence>()
                .HasOne<Customer>(u => u.Customer)
                .WithOne(c => c.Residence)
                .HasForeignKey<Residence>(c => c.CustomerId);

            modelBuilder.Entity<Residence>()
                .HasOne<ResidenceLocation>(u => u.ResidenceLocation)
                .WithOne(c => c.Residence)
                .HasForeignKey<Residence>(c => c.ResidenceLocationId);

            modelBuilder.Entity<Residence>()
                .HasMany<Device>(u => u.Devices)
                .WithOne(c => c.Residence)
                .HasForeignKey(c => c.ResidenceId);

            modelBuilder.Entity<Device>()
                .HasOne<DeviceInfo>(u => u.DeviceInfo)
                .WithOne(c => c.Device)
                .HasForeignKey<Device>(c => c.DeviceInfoId);

            modelBuilder.Entity<Device>()
                .HasMany<ActivitySession>(u => u.ActivitySessions)
                .WithOne(c => c.Device)
                .HasForeignKey(c => c.DeviceId);
        }
    }
}
