using System;
using System.Collections.Generic;
using System.Linq;
using DameChales.API.DAL.Common.Entities;
using DameChales.API.DAL.Common.Repositories;
using DameChales.API.DAL.Memory;
using DameChales.API.DAL.Memory.Repositories;
using DameChales.Common.Enums;
using Newtonsoft.Json;

namespace DameChales.API.DAL.IntegrationTests;


//todo check if it does a make sense becouse I am not sure about Restaurant entity and other entities
public class InMemoryDatabaseFixture : IDatabaseFixture
{   
    public FoodAmountEntity? GetFoodAmountDirectly(Guid foodAmountId)
    {
        var foodAmount = inMemoryStorage.Value.FoodAmounts.SingleOrDefault(t=>t.Id == foodAmountId);

        return DeepClone(foodAmount);
    }

    public RestaurantEntity? GetRestaurantDirectly(Guid restaurantId)
    {
        var restaurant = inMemoryStorage.Value.Restaurants.SingleOrDefault(t=>t.Id == restaurantId);

        return DeepClone(restaurant);
    }
    
    public OrderEntity? GetOrderDirectly(Guid orderId)
    {
        var order = inMemoryStorage.Value.Orders.SingleOrDefault(t=>t.Id == orderId);
        if(order == null)
        {
            return null;
        }
        order.FoodAmounts = inMemoryStorage.Value.FoodAmounts.Where(t => t.OrderGuid == orderId).ToList();

        return DeepClone(order);

    }

    private T DeepClone<T>(T input)
    {
        var json = JsonConvert.SerializeObject(input);
        return JsonConvert.DeserializeObject<T>(json);
    }

    public IRestaurantRepository GetRestaurantRepository()
    {
        return new RestaurantRepository(inMemoryStorage.Value,);
    }

    public IList<Guid> FoodGuids { get; } = new List<Guid>
    {
        new ("df935025-8709-4040-a2bb-b6f97cb416dc"),
        new ("23b3901d-7d4f-4213-9cf0-112348f56238")
    };

    public IList<Guid> FoodAmountGuids { get; } = new List<Guid>
    {
        new ("0d4fa150-ad80-4d46-a511-4c666166ec5e"),
        new ("87833e36-05ba-4d6b-900b-fe5ace88dbd8")
    };

    public IList<Guid> OrderGuids { get; } = new List<Guid>
    {
        new ("fabde3cd-eefe-443f-baf6-3d96cc2cbf2e")
    };

    public IList<Guid> RestaurantGuids { get; } = new List<Guid>
    {
        new ("gacce2cd-hhfe-123f-fag3-3d96aaaabf2e")
    };

    //ponecham
    private readonly Lazy<Storage> inMemoryStorage;

    //ponecham
    public InMemoryDatabaseFixture()
    {
        inMemoryStorage = new Lazy<Storage>(CreateInMemoryStorage);
    }

    //ponecham
    private Storage CreateInMemoryStorage()
    {
        var storage = new Storage(false);
        SeedStorage(storage);
        return storage;
    }

    //todo add restaurants , food and orders 
    private void SeedStorage(Storage storage)
    {
            storage.Foods.Add(new FoodEntity()
            {
                Id = FoodGuids[0],
                Name = "orechy",
                PhotoURL = "https://upload.wikimedia.org/wikipedia/commons/thumb/5/5e/Chicken_egg_2009-06-04.jpg/428px-Chicken_egg_2009-06-04.jpg",
                Description = "Popis vajicek s orechy.",
                Price = 150,
                RestaurantGuid = RestaurantGuids[0],
                alergens = new HashSet<Alergens>() { Alergens.Nuts }
            });

            storage.Foods.Add(new FoodEntity
            {
                Id = FoodGuids[1],
                Name = "Slehacka",
                PhotoURL = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/25/Onion_on_White.JPG/480px-Onion_on_White.JPG",
                Description = "Popis cibule na slehacce.",
                Price = 100.5,
                RestaurantGuid = RestaurantGuids[0],
                alergens = new HashSet<Alergens>() { Alergens.Milk }
            });
        
            storage.FoodAmounts.Add(new FoodAmountEntity
            {
                Id = FoodAmountGuids[0],
                OrderGuid = OrderGuids[0],
                Amount = 1,
                Note = "poznamka"
            });

            storage.FoodAmounts.Add(new FoodAmountEntity
            {
                Id = FoodAmountGuids[1],
                OrderGuid = OrderGuids[0],
                Amount = 2
            });

            storage.Orders.Add(new OrderEntity
            {
                Name = "Peter Parker",
                Id = OrderGuids[0],
                RestaurantGuid = RestaurantGuids[0],
                DeliveryTime = TimeSpan.FromMinutes(15),
                Note = "Poznamka k objednavce.",
                Status = OrderStatus.Accepted
            });
        

            storage.Restaurants.Add(new RestaurantEntity
            {
                Id = RestaurantGuids[0],
                Name = "SkvelaRestaurace",
                Description="Mame nejlepsi vajicka",
                LogoURL= "https://m.facebook.com/eggotruckbrno/",
                Address= "Dvořákova 12, Brno, Czech Republic",
                GPSCoordinates= "49.195942, 16.611404"
            });
    }
}
