using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using DameChales.Common.Enums;

namespace DameChales.API.DAL.Common.Entities
{
    public record OrderEntity : EntityBase
    {
        public Guid RestaurantGuid { get; set; }
        public RestaurantEntity? RestaurantEntity { get; set; }
        public required TimeSpan DeliveryTime { get; set; }
        public required string Note { get; set; }
        public required OrderStatus Status { get; set; }

        public ICollection<FoodAmountEntity> FoodAmounts { get; set; } = new List<FoodAmountEntity>();

        public OrderEntity(Guid id, Guid restaurantId, TimeSpan deliveryTime, string note, OrderStatus status, List<FoodAmountEntity> foodAmounts) :
            base(id)
        {
            DeliveryTime = deliveryTime;
            RestaurantGuid = restaurantId;
            DeliveryTime = deliveryTime;
            Note = note;
            Status = status;
            FoodAmounts = foodAmounts;
        }

        public OrderEntity() :
            this(Guid.Empty, Guid.Empty, TimeSpan.Zero, string.Empty, OrderStatus.Accepted, new List<FoodAmountEntity>())
        {
        }

    }

    public class OrderEntityMapperProfile : Profile
    {
        public OrderEntityMapperProfile()
        {
            CreateMap<OrderEntity, OrderEntity>();
        }
    }
}
