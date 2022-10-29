using System;
using System.Collections.Generic;
using System.Linq;
using DameChales.API.DAL.Common.Entities;
using DameChales.Common.Enums;
using Xunit;

namespace DameChales.API.DAL.IntegrationTests;

public class FoodRepositoryTests
{
    public FoodRepositoryTests()
    {
        dbFixture = new InMemoryDatabaseFixture();
    }

    private readonly IDatabaseFixture dbFixture;

    [Fact]
    public void GetById_Returns_Requested_Order_Including_Their_OrdersAmounts()
    {
        //arrange
        var orderRepository = dbFixture.GetOrderRepository();

        //act
        var order = orderRepository.GetById(dbFixture.OrderGuids[0]);

        //assert
        Assert.Equal(2, order.FoodAmounts.Count);

        var orderAmount1 =
            Assert.Single(order.FoodAmounts.Where(entity => entity.Id == dbFixture.FoodAmountGuids[0]));
        var orderAmount2 =
            Assert.Single(order.FoodAmounts.Where(entity => entity.Id == dbFixture.FoodAmountGuids[1]));

        Assert.Equal(dbFixture.OrderGuids[0], orderAmount1.OrderGuid);
        Assert.Equal(dbFixture.OrderGuids[0], orderAmount2.OrderGuid);

        Assert.NotNull(orderAmount1.FoodEntity);
        Assert.Equal("orechy",orderAmount1.FoodEntity.Name);

        Assert.NotNull(orderAmount2.FoodEntity);
        Assert.Equal("Slehacka", orderAmount2.FoodEntity.Name);

    }

    [Fact]
    public void GetById_Returns_Requested_Restaurant()
    {
        //arrange
        var restaurantRepository = dbFixture.GetRestaurantRepository();

        //act
        var restaurant = restaurantRepository.GetById(dbFixture.RestaurantGuids[0]);

        Assert.Equal(dbFixture.RestaurantGuids[0], restaurant.Id);
        Assert.Equal("Pizza Forte", restaurant.Name);
    }

    [Fact]
    public void Insert_Saves_Order_And_FoodAmounts()
    {
        //arrange
        var orderRepository = dbFixture.GetOrderRepository();

        var foodAmountId = Guid.NewGuid();
        var restaurantId = Guid.NewGuid();
        var duration = TimeSpan.FromMinutes(5);
        var status = OrderStatus.Accepted;
        var orderId = Guid.NewGuid();

        var newOrder = new OrderEntity {
            Id = orderId,
            Name = "Name",
            DeliveryTime = duration,
            RestaurantGuid = restaurantId,
            Note = "Note",
            Status = status,
            FoodAmounts = new List<FoodAmountEntity>()
            {
                new FoodAmountEntity
                {
                    Id= foodAmountId,
                    OrderGuid = orderId,
                    Amount = 3,
                    Note = "Note"
                }
            }
        };

        //act 
        orderRepository.Insert(newOrder);

        //assert
        var order = dbFixture.GetOrderDirectly(orderId);
        Assert.NotNull(order);
        Assert.Equal(status, order.Status);
        Assert.Equal(duration, order.DeliveryTime);

        var ingredientAmount = dbFixture.GetFoodAmountDirectly(foodAmountId);
        Assert.NotNull(ingredientAmount);
    }

    [Fact]
    public void Update_Saves_NewFoodAmount()
    {
        //arrange
        var orderRepository = dbFixture.GetOrderRepository();

        var orderId = dbFixture.OrderGuids[0];
        var order = dbFixture.GetOrderDirectly(orderId);
        var foodAmountId = Guid.NewGuid();

        var newOrderAmount =
            new FoodAmountEntity
            {
                Id = foodAmountId,
                FoodGuid = dbFixture.FoodGuids[0],
                Amount = 3,
                OrderGuid = orderId,
                OrderEntity=order,
            };

        //act
        order.FoodAmounts.Add(newOrderAmount);
        orderRepository.Update(order);

        //assert
        var orderFromDb = dbFixture.GetOrderDirectly(orderId);
        Assert.NotNull(orderFromDb);
        Assert.Single(orderFromDb.FoodAmounts.Where(t=>t.Id == foodAmountId));

        var foodAmountFromDb =dbFixture.GetFoodAmountDirectly(foodAmountId);
        Assert.NotNull(foodAmountFromDb);
    }

    [Fact]
    public void Update_Also_Updates_FoodAmount()
    {
        //arrange 
        var orderRepository = dbFixture.GetOrderRepository();

        var orderId = dbFixture.OrderGuids[0];
        var order = dbFixture.GetOrderDirectly(orderId);
        var foodAmount = order.FoodAmounts.First();

        //act
        foodAmount.Amount = 3;
        orderRepository.Update(order);

        //assert
        var orderFromDb = dbFixture.GetOrderDirectly(orderId);
        Assert.NotNull(orderFromDb);
        var foodAmountFromDb = orderFromDb.FoodAmounts.First();
        Assert.Equal(foodAmount.Id, foodAmountFromDb.Id);
        Assert.Equal(3,foodAmountFromDb.Amount);
    }

    [Fact]
    public void Update_Removes_IngredientAmount()
    {
        //arrange 
        var orderRepository = dbFixture.GetOrderRepository();

        var orderId = dbFixture.OrderGuids[0];
        var order = dbFixture.GetOrderDirectly(orderId);
        var foodAmountId = order.FoodAmounts.First().Id;

        //act
        order.FoodAmounts.Clear();
        orderRepository.Update(order);

        //assert
        var orderFromDb = dbFixture.GetOrderDirectly(orderId);
        Assert.NotNull(orderFromDb);
        Assert.Empty(orderFromDb.FoodAmounts);

        var foodAmountFromDb = dbFixture.GetFoodAmountDirectly(foodAmountId);
        Assert.Null(foodAmountFromDb);
    }

}
