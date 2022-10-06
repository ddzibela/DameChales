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

    }

    public class RestaurantMapperProfile : Profile
    {
        public RestaurantMapperProfile()
        {
            CreateMap<RestaurantEntity, RestaurantEntity>();
        }
    }
}
