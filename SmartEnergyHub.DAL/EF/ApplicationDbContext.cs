﻿using Microsoft.EntityFrameworkCore;
using SmartEnergyHub.DAL.Entities.APIUser;

namespace SmartEnergyHub.DAL.EF
{
    internal class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { 
            Database.EnsureCreated();
        }

        public ApplicationDbContext() { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>();
        }
    }
}
