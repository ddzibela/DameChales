using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;

namespace DameChales.API.DAL.Common.Entities
{
    public record OrderEntity : EntityBase
    {
        public Guid RestaurantGuid { get; set; }
        public RestaurantEntity? RestaurantEntity { get; set; }
        public required DateTime DeliveryTime { get; set; }
        public required string Note { get; set; }
        //TODO add order status
    }

    public class OrderEntityMapperProfile : Profile
    {
        public OrderEntityMapperProfile()
        {
            CreateMap<OrderEntity, OrderEntity>();
        }
    }
}
