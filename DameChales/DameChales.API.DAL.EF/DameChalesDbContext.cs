using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DameChales.API.DAL.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace DameChales.API.DAL.EF
{
    public class DameChalesDbContext : DbContext
    {
        public DbSet<FoodAmountEntity> FoodAmounts { get; set; } = null!;
        public DbSet<FoodEntity> Foods { get; set; } = null!;
        public DbSet<OrderEntity> Orders { get; set; } = null!;
        public DbSet<RestaurantEntity> Restaurants { get; set; } = null!;
        
        public DameChalesDbContext(DbContextOptions<DameChalesDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RestaurantEntity>()
                .HasMany(restaurantEntity => restaurantEntity.Orders)
                .WithOne(orderEntity => orderEntity.RestaurantEntity)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RestaurantEntity>()
                .HasMany(restaurantEntity => restaurantEntity.Foods)
                .WithOne(foodEntity => foodEntity.RestaurantEntity)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FoodEntity>()
                .HasMany(typeof(FoodAmountEntity))
                .WithOne(nameof(FoodAmountEntity.FoodEntity))
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderEntity>()
                .HasMany(typeof(FoodAmountEntity))
                .WithOne(nameof(FoodAmountEntity.OrderEntity))
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
