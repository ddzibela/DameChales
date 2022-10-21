using System;
using System.Collections.Generic;
using DameChales.API.DAL.Common.Entities;
using DameChales.API.DAL.Common.Repositories;

namespace DameChales.API.DAL.IntegrationTests;

public interface IDatabaseFixture
{
    //todo change to our solution
    //restaurat-recipe
    //ingredient-order

    FoodAmountEntity? GetFoodAmountDirectly(Guid foodAmountEntity);
    FoodEntity? GetFoodRepository();
    OrderEntity? GetOrderDirectly(Guid orderId);
    RestaurantEntity? GetRestaurantDirectly(Guid restaurantId);

    /*
    IngredientAmountEntity? GetIngredientAmountDirectly(Guid ingredientAmountId);
    RecipeEntity? GetRecipeDirectly(Guid recipeId);
    IRecipeRepository GetRecipeRepository();
    */

    IList<Guid> foodGuids { get; }
    IList<Guid> foodAmountGuids { get; }
    IList<Guid> RecipeGuids { get; }
    IList<Guid> restaurantGuids { get; }

}