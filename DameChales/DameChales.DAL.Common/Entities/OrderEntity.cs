using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using AutoMapper;
using DameChales.Common.Enums;

namespace DameChales.API.DAL.Common.Entities
{
    public record OrderEntity : EntityBase
    {
        public Guid RestaurantGuid { get; set; }
        public RestaurantEntity? RestaurantEntity { get; set; }
        public required DateTime DeliveryTime { get; set; }
        public required string Name { get; set; }
        public required string? Note { get; set; }
        public required string Address { get; set; }
        public required OrderStatus Status { get; set; }

        public ICollection<FoodAmountEntity> FoodAmounts { get; set; } = new List<FoodAmountEntity>();

        public OrderEntity(string name, Guid id, Guid restaurantId, DateTime deliveryTime, string note, string address,OrderStatus status, List<FoodAmountEntity> foodAmounts) :
            base(id)
        {
            Name = name;
            DeliveryTime = deliveryTime;
            RestaurantGuid = restaurantId;
            Note = note;
            Address = address;
            Status = status;
            FoodAmounts = foodAmounts;
        }

        public OrderEntity() :
            this(string.Empty,Guid.Empty, Guid.Empty, DateTime.MinValue, string.Empty, string.Empty, OrderStatus.Accepted, new List<FoodAmountEntity>())
        {
        }

    }

}
