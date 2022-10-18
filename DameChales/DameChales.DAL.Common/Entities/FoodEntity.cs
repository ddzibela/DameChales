using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using DameChales.Common.Enums;

namespace DameChales.API.DAL.Common.Entities
{
    public record FoodEntity : EntityBase
    {
        public required string Name { get; set; }
        public required string PhotoURL { get; set; }
        public required string Description { get; set; }
        public required double Price { get; set; }

        public Guid RestaurantGuid { get; set; }
        public RestaurantEntity? Restaurant { get; set; }
        public ICollection<Alergens> alergens { get; set; } = new HashSet<Alergens>();

        public FoodEntity(Guid id, string name, string photoURL, string description, double price, Guid restaurant, HashSet<Alergens> alergens) :
            base(id)
        {
            Name = name;
            PhotoURL = photoURL;
            Description = description;
            Price = price;
            RestaurantGuid = restaurant;
            this.alergens = alergens;
        }

        public FoodEntity() :
            this(Guid.Empty, string.Empty, string.Empty, string.Empty, double.NaN, Guid.Empty, new HashSet<Alergens>())
        {
        }

    }

    public class FoodEntityMapperProfile : Profile
    {
        public FoodEntityMapperProfile()
        {
            CreateMap<FoodEntity, FoodEntity>();
        }
    }
}
