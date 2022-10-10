using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;

namespace DameChales.API.DAL.Common.Entities
{
    public record RestaurantEntity : EntityBase
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string LogoURL { get; set; }
        public required string Address { get; set; }
        public required string GPSCoordinates { get; set; }

        public ICollection<OrderEntity> Orders { get; set; } = new List<OrderEntity>();
        public ICollection<FoodEntity> Foods { get; set; } = new List<FoodEntity>();
    }

    public class RestaurantMapperProfile : Profile
    {
        public RestaurantMapperProfile()
        {
            CreateMap<RestaurantEntity, RestaurantEntity>();
        }
    }
}
