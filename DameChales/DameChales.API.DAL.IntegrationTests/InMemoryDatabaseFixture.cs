using System;
using System.Collections.Generic;
using System.Linq;

namespace DameChales.API.DAL.IntegrationTests;


//todo check if it does a make sense becouse I am not sure about Restaurant entity and other entities
public class InMemoryDatabaseFixture : IDatabaseFixture
{
    //restaurat-recipe
    //ingredient-order

    FoodAmountEntity? GetFoodAmountDirectly(Guid foodAmountId)
    {
        var foodAmount = inMemoryStorage.Value.FoodsAmounts.SingleOrDefault(t=>t.Id == foodAmountId);

        return foodAmount;
    }

    /*
    public IngredientAmountEntity? GetIngredientAmountDirectly(Guid ingredientAmountId)
    {
        var ingredientAmount = inMemoryStorage.Value.IngredientAmounts.SingleOrDefault(t => t.Id == ingredientAmountId);

        return DeepClone(ingredientAmount);
    }
    */
    RestaurantEntity? GetRestaurantDirectly(Guid restaurantId)
    {
        var restaurant = inMemoryStorage.Value.Restaurants.SingleOrDefault(t=>t?.Id == restaurantId);
        if (restaurant == null)
        {
            return null;
        }

        restaurant.FoodsAmounts = inMemoryStorage.Value.FoodsAmounts.Where(t => t.RestaurantId == restaurantId).ToList();

        return DeepClone(restaurant);
    }
    
    OrderEntity? GetOrderDirectly(Guid orderId)
    {
        var order = inMemoryStorage.Value.Orders.SingleOrDefault(t=>t.Id == orderId);
        if(order == null)
        {
            return null;
        }
        order.FoodsAmounts = inMemoryStorage.Value.FoodsAmounts.Where(t => t.OrderId== orderId).ToList();

        return DeepClone(order);

    }


    /*
    public RecipeEntity? GetRecipeDirectly(Guid recipeId)
    {
        var recipe = inMemoryStorage.Value.Recipes.SingleOrDefault(t => t.Id == recipeId);
        if (recipe == null)
        {
            return null;
        }

        recipe.IngredientAmounts = inMemoryStorage.Value.IngredientAmounts.Where(t => t.RecipeId == recipeId).ToList();


        return DeepClone(recipe);
    }
    */

    IFoodRepository? GetFoodRepository()
    {
        return new FoodRepository(inMemoryStorage.Value);
    }


    /*
    public IRecipeRepository GetRecipeRepository()
    {
        return new RecipeRepository(inMemoryStorage.Value);
    }
    */


    //ponecham
    private T DeepClone<T>(T input)
    {
        var json = JsonConvert.SerializeObject(input);
        return JsonConvert.DeserializeObject<T>(json);
    }

    /*
    public IList<Guid> IngredientGuids { get; } = new List<Guid>
    {
        new("df935095-8709-4040-a2bb-b6f97cb416dc"), new("23b3902d-7d4f-4213-9cf0-112348f56238")
    };

    public IList<Guid> IngredientAmountGuids { get; } = new List<Guid>
    {
        new("0d4fa150-ad80-4d46-a511-4c666166ec5e"), new("87833e66-05ba-4d6b-900b-fe5ace88dbd8")
    };

    public IList<Guid> RecipeGuids { get; } = new List<Guid> { new("fabde0cd-eefe-443f-baf6-3d96cc2cbf2e") };
    */
        public IList<Guid> foodGuids = new List<Guid>
        {
            new ("df935025-8709-4040-a2bb-b6f97cb416dc"),
            new ("23b3901d-7d4f-4213-9cf0-112348f56238")
        };

        public IList<Guid> foodAmountGuids = new List<Guid>
        {
            new ("0d4fa150-ad80-4d46-a511-4c666166ec5e"),
            new ("87833e36-05ba-4d6b-900b-fe5ace88dbd8")
        };

        public IList<Guid> orderGuids = new List<Guid>
        {
            new ("fabde3cd-eefe-443f-baf6-3d96cc2cbf2e")
        };

        public IList<Guid> restaurantGuids = new List<Guid>
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
                private void SeedFoods()
        {

            Foods.Add(new FoodEntity()
            {
                Id = foodGuids[0],
                Name = "orechy",
                PhotoURL = "https://upload.wikimedia.org/wikipedia/commons/thumb/5/5e/Chicken_egg_2009-06-04.jpg/428px-Chicken_egg_2009-06-04.jpg",
                Description = "Popis vajicek s orechy.",
                Price = 150,
                RestaurantGuid = restaurantGuids[0],
                alergens = new HashSet<Alergens>() { Alergens.Nuts }
            });

            Foods.Add(new FoodEntity
            {
                Id = foodGuids[1],
                Name = "Slehacka",
                PhotoURL = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/25/Onion_on_White.JPG/480px-Onion_on_White.JPG",
                Description = "Popis cibule na slehacce.",
                Price = 100.5,
                RestaurantGuid = restaurantGuids[0],
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
