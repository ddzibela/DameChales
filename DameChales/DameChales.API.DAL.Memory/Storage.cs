using System;
using System.Collections.Generic;
using DameChales.API.DAL.Common.Entities;
using DameChales.Common.Enums;

namespace DameChales.API.DAL.Memory
{
    public class Storage
    {
        private readonly IList<Guid> foodGuids = new List<Guid>
        {
            new ("df935095-8709-4040-a2bb-b6f97cb416dc"),
            new ("23b3902d-7d4f-4213-9cf0-112348f56238")
        };

        private readonly IList<Guid> foodAmountGuids = new List<Guid>
        {
            new ("0d4fa150-ad80-4d46-a511-4c666166ec5e"),
            new ("87833e66-05ba-4d6b-900b-fe5ace88dbd8")
        };

        private readonly IList<Guid> orderGuids = new List<Guid>
        {
            new ("fabde0cd-eefe-443f-baf6-3d96cc2cbf2e")
        };

        private readonly IList<Guid> restaurantGuids = new List<Guid>
        {
            new ("gacce0cd-hhfe-123f-fag3-3d96aaaabf2e")
        };

        public IList<FoodEntity> Foods { get; } = new List<FoodEntity>();
        public IList<FoodAmountEntity> FoodAmounts { get; } = new List<FoodAmountEntity>();
        public IList<OrderEntity> Orders { get; } = new List<OrderEntity>();
        public IList<RestaurantEntity> Restaurants { get; } = new List<RestaurantEntity>();

        public Storage(bool seedData = true)
        {
            if (seedData)
            {
                SeedFoods();
                SeedFoodAmounts();
                SeedOrders();
                SeedRestaurants();
            }
        }

        private void SeedFoods()
        {
            Foods.Add(new FoodEntity
            {
                Id = foodGuids[0],
                Name = "Vajicka s orechy",
                PhotoURL = "https://upload.wikimedia.org/wikipedia/commons/thumb/5/5e/Chicken_egg_2009-06-04.jpg/428px-Chicken_egg_2009-06-04.jpg",
                Description = "Popis vajicek s orechy.",
                Price = 150,
                RestaurantGuid = restaurantGuids[0],

                //todo alergens.Add(Alergens.Nuts),

            });

            Foods.Add(new FoodEntity
            {
                Id = foodGuids[1],
                Name = "Cibule na slehacce",
                PhotoURL = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/25/Onion_on_White.JPG/480px-Onion_on_White.JPG",
                Description = "Popis cibule na slehacce.",
                Price = 100.5,
                RestaurantGuid = restaurantGuids[0],
                //todo alergens.Add(Alergens.Nuts),

            });
        }
   
        private void SeedFoodAmounts()
        {
            FoodAmounts.Add(new FoodAmountEntity
            {
                Id = foodAmountGuids[0],
                OrderGuid = orderGuids[0],
                Amount = 1,
                Note = "poznamka"
    });

            FoodAmounts.Add(new FoodAmountEntity
            {
                Id = foodAmountGuids[1],
                OrderGuid = orderGuids[0],
                Amount = 2
            });
        }

        private void SeedOrders()
        {
            Orders.Add(new OrderEntity
            {
                Id = orderGuids[0],
                RestaurantGuid = restaurantGuids[0],
                DeliveryTime = TimeSpan.FromMinutes(15),
                Note = "Poznamka k objednavce.",
                Status = OrderStatus.Accepted
            });
        }

        private void SeedRestaurants()
        {
            Restaurants.Add(new RestaurantEntity
            {
                Id = restaurantGuids[0],
                Name = "SkvelaRestaurace",
                Description="Mame nejlepsi vajicka",
                LogoURL= "https://m.facebook.com/eggotruckbrno/",
                Address= "Dvořákova 12, Brno, Czech Republic",
                GPSCoordinates= "49.195942, 16.611404"

            });
        }

    }
}
