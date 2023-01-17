using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DameChales.Common.Models.Resources.Texts;

namespace DameChales.Common.Models
{
    public record RestaurantDetailModel : IWithId
    {
        public required Guid Id { get; init; }

        [Required(ErrorMessageResourceName = nameof(RestaurantDetailModelResources.Name_Required_ErrorMessage), ErrorMessageResourceType = typeof(RestaurantDetailModelResources))]
        public required string Name { get; set; }

        [Required(ErrorMessageResourceName = nameof(RestaurantDetailModelResources.Description_Required_ErrorMessage), ErrorMessageResourceType = typeof(RestaurantDetailModelResources))]
        public required string? Description { get; set; }
        public required string? PhotoURL { get; set; }

        [Required(ErrorMessageResourceName = nameof(RestaurantDetailModelResources.Address_Required_ErrorMessage), ErrorMessageResourceType = typeof(RestaurantDetailModelResources))]
        public required string Address { get; set; }
        public required string GPSCoordinates { get; set; }

        public IList<FoodListModel> Foods {get; set; } = new List<FoodListModel>();
        public IList<OrderListModel> Orders { get; set; } = new List<OrderListModel>();
    }
}
