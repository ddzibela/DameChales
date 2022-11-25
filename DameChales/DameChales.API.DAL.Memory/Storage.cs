using System;
using System.Collections.Generic;
using DameChales.API.DAL.Common.Entities;
using DameChales.Common.Enums;
using Microsoft.Data.SqlClient;

namespace DameChales.API.DAL.Memory
{
    public class Storage
    {
        private readonly IList<Guid> foodGuids = new List<Guid>
        {
        new ("96103111-393B-46B8-8B4F-EC82212CFFBF"),
        new ("82BFF672-382C-49E9-ACA2-52DD028414A3")
        };

        private readonly IList<Guid> foodAmountGuids = new List<Guid>
        {
        new ("67ECBE97-BA81-490D-9F9A-11C4832B4E94"),
        new ("3B9F8A14-B6ED-4701-AB35-B05096C2FCCF")
        };

        private readonly IList<Guid> orderGuids = new List<Guid>
        {
        new ("E184748D-B151-4129-83F9-F2AC2486FA55")
        };

        private readonly IList<Guid> restaurantGuids = new List<Guid>
        {
        new ("75970373-0AFA-4C9B-9BC3-2655F3C1EFE0")
        };

        public IList<RestaurantEntity> Restaurants { get; } = new List<RestaurantEntity>();
        public IList<FoodEntity> Foods { get; } = new List<FoodEntity>();
        public IList<FoodAmountEntity> FoodAmounts { get; } = new List<FoodAmountEntity>();
        public IList<OrderEntity> Orders { get; } = new List<OrderEntity>();

        public Storage(bool seedData = true)
        {
            if (seedData)
            {
                SeedRestaurants();
                SeedFoods();
                SeedRestaurantFoods();
                SeedFoodAmounts();
                SeedOrders();
                SeedRestaurantOrders();
            }
        }

        private void SeedFoods()
        {

            Foods.Add(new FoodEntity()
            {
                Id = foodGuids[0],
                Name = "Vajicka s orechy",
                PhotoURL = "https://upload.wikimedia.org/wikipedia/commons/thumb/5/5e/Chicken_egg_2009-06-04.jpg/428px-Chicken_egg_2009-06-04.jpg",
                Description = "Popis vajicek s orechy.",
                Price = 150,
                RestaurantGuid = restaurantGuids[0],
                RestaurantEntity = Restaurants.Single(e => e.Id == restaurantGuids[0]),
                alergens = new HashSet<Alergens>() { Alergens.Nuts }
            }); 

            Foods.Add(new FoodEntity
            {
                Id = foodGuids[1],
                Name = "Cibule na slehacce",
                PhotoURL = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/25/Onion_on_White.JPG/480px-Onion_on_White.JPG",
                Description = "Popis cibule na slehacce.",
                Price = 100.5,
                RestaurantGuid = restaurantGuids[0],
                RestaurantEntity = Restaurants.Single(e => e.Id == restaurantGuids[0]),
                alergens = new HashSet<Alergens>() { Alergens.Milk }
            });
        }

        private void SeedFoodAmounts()
        {
            FoodAmounts.Add(new FoodAmountEntity
            {
                Id = foodAmountGuids[0],
                OrderGuid = orderGuids[0],
                Amount = 1,
                Note = "poznamka",
                FoodGuid = foodGuids[0],
                FoodEntity = Foods.Single(f => f.Id == foodGuids[0]),
            });

            FoodAmounts.Add(new FoodAmountEntity
            {
                Id = foodAmountGuids[1],
                OrderGuid = orderGuids[0],
                Amount = 2,
                FoodGuid = foodGuids[1],
                FoodEntity = Foods.Single(f => f.Id == foodGuids[1])
            });
        }

        private void SeedOrders()
        {
            Orders.Add(new OrderEntity
            {
                Name = "Dominik Petrik",
                Id = orderGuids[0],
                RestaurantGuid = restaurantGuids[0],
                RestaurantEntity = Restaurants.Single(f => f.Id == restaurantGuids[0]),
                DeliveryTime = DateTime.Now.AddHours(1),
                Note = "Poznamka k objednavce.",
                Status = OrderStatus.Accepted,
                FoodAmounts = new List<FoodAmountEntity>
                {
                    FoodAmounts[0],
                    FoodAmounts[1]
                }
            });

            FoodAmounts[0].OrderEntity = Orders[0];
            FoodAmounts[1].OrderEntity = Orders[0];
        }

        private void SeedRestaurants()
        {
            Restaurants.Add(new RestaurantEntity
            {
                Id = restaurantGuids[0],
                Name = "SkvelaRestaurace",
                Description = "Mame nejlepsi vajicka",
                LogoURL = "https://m.facebook.com/eggotruckbrno/",
                Address = "Dvořákova 12, Brno, Czech Republic",
                GPSCoordinates = "49.195942, 16.611404",
                Orders = new List<OrderEntity>(),
                Foods = new List<FoodEntity>()
            });
        }

        private void SeedRestaurantFoods()
        {
            var restaurant = Restaurants.Single(e => e.Id == restaurantGuids[0]);
            restaurant.Foods.Add(Foods.Single(f => f.Id == foodGuids[0]));
            restaurant.Foods.Add(Foods.Single(f => f.Id == foodGuids[1]));
        }

        private void SeedRestaurantOrders()
        {
            var restaurant = Restaurants.Single(e => e.Id == restaurantGuids[0]);
            restaurant.Orders.Add(Orders.Single(f => f.Id == orderGuids[0]));
        }
    }
}
