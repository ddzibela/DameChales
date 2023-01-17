using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
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

    public IOrderRepository GetOrderRepository()
    {
        return new OrderRepository(inMemoryStorage.Value);
    }

    public IRestaurantRepository GetRestaurantRepository()
    {
        var options = new MapperConfiguration(cfg => cfg.CreateMap<RestaurantEntity, RestaurantEntity>());
        return new RestaurantRepository(inMemoryStorage.Value, new Mapper(options));
    }

    public IList<Guid> FoodGuids { get; } = new List<Guid>
    {
        new ("96103111-393B-46B8-8B4F-EC82212CFFBF"),
        new ("82BFF672-382C-49E9-ACA2-52DD028414A3")
    };

    public IList<Guid> FoodAmountGuids { get; } = new List<Guid>
    {
        new ("67ECBE97-BA81-490D-9F9A-11C4832B4E94"),
        new ("3B9F8A14-B6ED-4701-AB35-B05096C2FCCF")
    };

    public IList<Guid> OrderGuids { get; } = new List<Guid>
    {
        new ("E184748D-B151-4129-83F9-F2AC2486FA55")
    };

    public IList<Guid> RestaurantGuids { get; } = new List<Guid>
    {
        new ("75970373-0AFA-4C9B-9BC3-2655F3C1EFE0")
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
                FoodGuid = FoodGuids[0],
                Amount = 1,
                Note = "poznamka"
            });

            storage.FoodAmounts.Add(new FoodAmountEntity
            {
                Id = FoodAmountGuids[1],
                OrderGuid = OrderGuids[0],
                FoodGuid = FoodGuids[1],
                Amount = 2
            });

            storage.Orders.Add(new OrderEntity
            {
                Name = "Peter Parker",
                Id = OrderGuids[0],
                RestaurantGuid = RestaurantGuids[0],
                DeliveryTime = DateTime.Now.AddHours(3),
                Note = "Poznamka k objednavce.",
                Address = "Adresa", 
                Status = OrderStatus.Accepted
            });
        

            storage.Restaurants.Add(new RestaurantEntity
            {
                Id = RestaurantGuids[0],
                Name = "Pizza Forte",
                Description="Mame nejlepsi vajicka",
                LogoURL= "https://m.facebook.com/eggotruckbrno/",
                Address= "Dvořákova 12, Brno, Czech Republic",
                GPSCoordinates= "49.195942, 16.611404"
            });
    }
}
