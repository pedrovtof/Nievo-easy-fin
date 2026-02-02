using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using auth.Models;

namespace auth.Data
{
    public abstract class AppDbContext : DbContext
    {
        protected AppDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<UserModels.User> Users { get; set; }
        public DbSet<UserModels.UserStatus> UserStatuses { get; set; }
        public DbSet<UserModels.UserType> UserTypes { get; set; }
        public DbSet<UserModels.UserPasswordHistory> UserPasswordHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }

    }
}