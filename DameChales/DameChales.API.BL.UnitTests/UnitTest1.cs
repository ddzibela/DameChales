using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DameChales.API.BL.Facades;
using DameChales.API.DAL.Common.Repositories;
using DameChales.Common.Enums;
using DameChales.Common.Models;
using DameChales.API.DAL.Common.Repositories;
using Moq;
using Xunit;

namespace CookBook.Api.BL.UnitTests;
public class RecipeFacadeTests
{
    [Fact]
    public void Delete_Calls_Correct_Method_On_Repository()
    {
        //arrange
        var repositoryMock = new Mock<IRestaurantRepository>(MockBehavior.Strict);
        repositoryMock.Setup(recipeRepository => recipeRepository.Remove(It.IsAny<Guid>()));

        var repository = repositoryMock.Object;
        var mapper = new Mock<IMapper>(MockBehavior.Strict).Object;
        var facade = new RestaurantFacade(repository, mapper);

        var itemId = Guid.NewGuid();
        //act
        facade.Delete(itemId);

        //assert
        repositoryMock.Verify(restaurantRepository => restaurantRepository.Remove(itemId));
    }

    [Fact]
    public void MergeFoodAmounts_Merges_Order_With_Multiple_FoodsAmounts_Of_Same_Food_And_Unit()
    {
        //arrange
        var facade = GetFacadeWithForbiddenDependencyCalls();

        var mergedOrderId = Guid.NewGuid();
        var order = new OrderDetailModel()
        {
            Id = Guid.NewGuid(),
            Name = "Name",
            Note = "Note",
            DeliveryTime = TimeSpan.FromSeconds(1),
            Status = OrderStatus.Accepted,
            FoodAmounts = new List<OrderFoodAmountDetailModel>()
            {
                new OrderFoodAmountDetailModel
                {
                    Id = Guid.NewGuid(),
                    Amount = 4,
                    Note = "Note",
                    Food = new FoodListModel
                    {
                        Id= Guid.NewGuid(),
                        Name= "Name",
                        PhotoURL = "Url",
                        Price = 40,
                    }
                },

                new OrderFoodAmountDetailModel
                {
                    Id = Guid.NewGuid(),
                    Amount = 5,
                    Note = "Note",
                    Food = new FoodListModel
                    {
                        Id= Guid.NewGuid(),
                        Name= "Name",
                        PhotoURL = "Url",
                        Price = 40,
                    }
                }

            }
        };

        //act
        facade.MergeFoodsAmounts(order);

        //assert
        var mergedOrder = Assert.Single(order.FoodAmounts);
        Assert.Equal(9, mergedOrder.Amount);
        Assert.Equal(mergedOrderId, mergedOrder.Food.Id);

    }

    //todo look at this in cookbook they try if it merge into one if they have different units we should look if it merged if it has the same price ?
    [Fact]
    public void MergeFoodsAmounts_Does_Not_Merge_Recipe_With_Multiple_FoodsAmounts_Of_Same_Food_And_Different_Price()
    {
        //arrange
        var facade = GetFacadeWithForbiddenDependencyCalls();

        var mergedFoodId = Guid.NewGuid();
        var FoodAmount1Id = Guid.NewGuid();
        var FoodAmount2Id = Guid.NewGuid();
        var order = new OrderDetailModel()
        {
            Id = Guid.NewGuid(),
            Name = "Name",
            Note = "Note",
            DeliveryTime = TimeSpan.FromSeconds(1),
            Status = OrderStatus.Accepted,
            FoodAmounts = new List<OrderFoodAmountDetailModel>()
            {
                new OrderFoodAmountDetailModel
                {
                    Id = FoodAmount1Id,
                    Amount = 4,
                    Note = "Note",
                    Food = new FoodListModel
                    {
                        Id= Guid.NewGuid(),
                        Name= "Name",
                        PhotoURL = "Url",
                        Price = 40,
                    }
                },

                new OrderFoodAmountDetailModel
                {
                    Id = FoodAmount2Id,
                    Amount = 5,
                    Note = "Note",
                    Food = new FoodListModel
                    {
                        Id= Guid.NewGuid(),
                        Name= "Name",
                        PhotoURL = "Url",
                        Price = 10,
                    }
                }

            }
        };

        //arrange
        facade.MergeFoodsAmounts(order);

        //assert
        Assert.Equal(2, order.FoodAmounts.Count);
        var FoodAmount1 = Assert.Single(order.FoodAmounts.Where(t => t.Id == FoodAmount1Id));
        var FoodAmount2 = Assert.Single(order.FoodAmounts.Where(t => t.Id == FoodAmount2Id));

        Assert.Equal(4, FoodAmount1.Amount);
        Assert.Equal(40, FoodAmount1.Food.Price);

        Assert.Equal(5, FoodAmount1.Amount);
        Assert.Equal(10, FoodAmount1.Food.Price);

    }

    //todo look at this in cookbook they try if it merge into one if they have different units we should look if it merged if it has the same price ?
    [Fact]
    public void MergeFoodsAmounts_Does_Not_Merge_Order_With_Multiple_FoodAmounts_Of_Different_Food_And_Same_Price()
    {
        //arrange
        var facade = GetFacadeWithForbiddenDependencyCalls();

        var notFoodId = Guid.NewGuid();
        var not1FoodId = Guid.NewGuid();
        var FoodAmount1Id = Guid.NewGuid();
        var FoodAmount2Id = Guid.NewGuid();
        var order = new OrderDetailModel()
        {
            Id = Guid.NewGuid(),
            Name = "Name",
            Note = "Note",
            DeliveryTime = TimeSpan.FromSeconds(1),
            Status = OrderStatus.Accepted,
            FoodAmounts = new List<OrderFoodAmountDetailModel>()
            {
                new OrderFoodAmountDetailModel
                {
                    Id = FoodAmount1Id,
                    Amount = 4,
                    Note = "Note",
                    Food = new FoodListModel
                    {
                        Id= notFoodId,
                        Name= "Name",
                        PhotoURL = "Url",
                        Price = 40,
                    }
                },

                new OrderFoodAmountDetailModel
                {
                    Id = FoodAmount2Id,
                    Amount = 5,
                    Note = "Note",
                    Food = new FoodListModel
                    {
                        Id= not1FoodId,
                        Name= "Name",
                        PhotoURL = "Url",
                        Price = 40,
                    }
                }

            }
        };

        facade.MergeIngredientAmounts(order);

        //assert 
        Assert.Equal(2, order.FoodAmounts.Count);
        var FoodAmount1 = Assert.Single(order.FoodAmounts.Where(t => t.Id == FoodAmount1Id));
        var FoodAmount2 = Assert.Single(order.FoodAmounts.Where(t => t.Id == FoodAmount2Id));

        Assert.Equal(4, FoodAmount1.Amount);
        Assert.Equal(40, FoodAmount1.Food.Price);

        Assert.Equal(5, FoodAmount2.Amount);
        Assert.Equal(40, FoodAmount2.Food.Price);

    }

    [Fact]
    public void MergeFoodsAmounts_Does_Not_Fail_When_Order_Has_No_Food()
    {
        //arrange
        var facade = GetFacadeWithForbiddenDependencyCalls();
        var order = new OrderDetailModel()
        {
            Id = Guid.NewGuid(),
            Name = "Name",
            Note = "Note",
            DeliveryTime = TimeSpan.FromMinutes(5),
            Status = OrderStatus.Accepted,
            FoodAmounts = new List<OrderFoodAmountDetailModel>()
            {
            }
        };
        //act
        facade.MergeFoodAmounts(order);

        //assert
        Assert.Empty(order.FoodAmounts);
    }

    //todo
    private static OrderFacade GetFacadeWithForbiddenDependencyCalls()
    {
        var repository = new Mock<IOrderRepository>(MockBehavior.Strict).Object;
        var mapper = new Mock<IMapper>(MockBehavior.Strict).Object;
        var facade = new OrderFacade(repository, mapper);
        return facade;
    }
}
