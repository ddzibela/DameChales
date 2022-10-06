using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;

namespace DameChales.API.DAL.Common.Entities
{
    public record FoodAmountEntity : EntityBase
    {
        public Guid FoodGuid { get; set; }
        public FoodEntity? FoodEntity { get; set; }
        public Guid OrderGuid { get; set; }
        public OrderEntity? OrderEntity { get; set; }
        public required int Amount { get; set; }
        public string? Note { get; set; }
    }

    public class FoodAmountEntityMapperProfile : Profile
    {
        public FoodAmountEntityMapperProfile()
        {
            CreateMap<FoodAmountEntity, FoodAmountEntity>();
        }
    }
}
