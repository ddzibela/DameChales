using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DameChales.API.DAL.Common.Entities;
using DameChales.API.DAL.Common.Repositories;
using DameChales.Common.Enums;
using DameChales.Common.Models;

namespace DameChales.API.BL.Facades
{
    public class OrderFacade : IOrderFacade
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;

        public OrderFacade(
            IOrderRepository orderRepository,
            IMapper mapper)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
        }
        public List<OrderListModel> GetAll()
        {
            var orderEntities = orderRepository.GetAll();
            return mapper.Map<List<OrderListModel>>(orderEntities);
        }

        public List<OrderListModel> GetByRestaurantId(Guid id)
        {
            var orderEntities = orderRepository.GetByRestaurantId(id);
            return mapper.Map<List<OrderListModel>>(orderEntities);
        }
        public List<OrderListModel> GetByFoodId(Guid id)
        {
            var orderEntities = orderRepository.GetByFoodId(id);
            return mapper.Map<List<OrderListModel>>(orderEntities);
        }
        public List<OrderListModel> GetByStatus(Guid restaurantId, OrderStatus status)
        {
            var orderEntities = orderRepository.GetByStatus(restaurantId, status);
            return mapper.Map<List<OrderListModel>>(orderEntities);
        }
        public OrderDetailModel? GetById(Guid id)
        {
            var orderEntity = orderRepository.GetById(id);
            return mapper.Map<OrderDetailModel>(orderEntity);
        }

        public Guid CreateOrUpdate(OrderDetailModel orderModel)
        {
            return orderRepository.Exists(orderModel.Id)
                ? Update(orderModel)!.Value
                : Create(orderModel);
        }

        public Guid Create(OrderDetailModel orderModel)
        {
            MergeFoodAmounts(orderModel);
            var orderEntity = mapper.Map<OrderEntity>(orderModel);
            return orderRepository.Insert(orderEntity);
        }

        public Guid? Update(OrderDetailModel orderModel)
        {
            MergeFoodAmounts(orderModel);

            var orderEntity = mapper.Map<OrderEntity>(orderModel);
            orderEntity.FoodAmounts = orderModel.FoodAmounts.Select(t =>
                new FoodAmountEntity(t.Id, t.FoodGuid, t.OrderGuid, t.Amount, t.Note)).ToList();
            var result = orderRepository.Update(orderEntity);
            return result;
        }

        public void MergeFoodAmounts(OrderDetailModel order)
        {
            var result = new List<OrderDetailFoodModel>();
            var foodAmountGroups = order.FoodAmounts.GroupBy(t => $"{t.FoodEntity.Id}");

            foreach (var foodAmountGroup in foodAmountGroups)
            {
                var foodAmountFirst = foodAmountGroup.First();
                var totalAmount = foodAmountGroup.Sum(t => t.Amount);
                var foodAmount = new OrderDetailFoodModel(foodAmountFirst.Id, totalAmount, foodAmountFirst.FoodEntity);

                result.Add(foodAmount);
            }

            order.FoodAmounts = result;
        }

        public void Delete(Guid id)
        {
            orderRepository.Remove(id);
        }
    }
}
