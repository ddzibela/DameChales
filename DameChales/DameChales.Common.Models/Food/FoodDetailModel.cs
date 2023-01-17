using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using DameChales.Common;
using DameChales.Common.Enums;
using DameChales.Common.Models.Resources.Texts;

namespace DameChales.Common.Models
{
    public record FoodDetailModel : IWithId
    {
        public required Guid Id { get; init; }

        [Required(ErrorMessageResourceName = nameof(FoodDetailModelResources.Name_Required_ErrorMessage), ErrorMessageResourceType = typeof(FoodDetailModelResources))]
        public required string Name { get; set; }
        public required string PhotoURL { get; set; }
        public required string Description { get; set; }

        [Required(ErrorMessageResourceName = nameof(FoodDetailModelResources.Price_Required_ErrorMessage), ErrorMessageResourceType = typeof(FoodDetailModelResources))]
        public required double Price { get; set; }
        public Guid? RestaurantGuid { get; set; }
        public ICollection<Alergens> alergens { get; set; } = new HashSet<Alergens>();
    }
}
