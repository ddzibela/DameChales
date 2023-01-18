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

        public RestaurantEntity(Guid id, string name, string description, string logoURL, string address, string GPSCoordinates, List<OrderEntity> orderEntities, List<FoodEntity> foodEntities) :
            base(id)
        {
            Name = name;
            Description = description;
            LogoURL = logoURL;
            Address = address;
            this.GPSCoordinates = GPSCoordinates;
            Orders = orderEntities;
            Foods = foodEntities;
        }

        public RestaurantEntity() :
            this(Guid.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, new List<OrderEntity>(), new List<FoodEntity>())
        {

        }
    }
}
