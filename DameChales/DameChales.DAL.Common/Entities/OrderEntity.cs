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
    }

    public class OrderEntityMapperProfile : Profile
    {
        public OrderEntityMapperProfile()
        {
            CreateMap<OrderEntity, OrderEntity>();
        }
    }
}
