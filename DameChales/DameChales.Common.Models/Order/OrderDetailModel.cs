using DameChales.Common.Enums;
using DameChales.Common.Models.Resources.Texts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DameChales.Common.Models
{
    public record OrderDetailModel : IWithId
    {
        public required Guid Id { get; init; }

        public Guid RestaurantGuid { get; set; }

        [Required(ErrorMessageResourceName = nameof(OrderDetailModelResources.Name_Required_ErrorMessage), ErrorMessageResourceType = typeof(OrderDetailModelResources))]
        public required string Name {get; set;}
        public required string? Note { get; set;}
        public required DateTime DeliveryTime { get; set; }
        public required OrderStatus Status { get; set; }

        [Required(ErrorMessageResourceName = nameof(OrderDetailModelResources.Address_Required_ErrorMessage), ErrorMessageResourceType = typeof(OrderDetailModelResources))]
        public required string Address { get; set; }

        [Required(ErrorMessageResourceName = nameof(OrderDetailModelResources.FoodAmount_Required_ErrorMessage), ErrorMessageResourceType = typeof(OrderDetailModelResources))]
        public IList<OrderFoodAmountDetailModel> FoodAmounts { get; set; } = new List<OrderFoodAmountDetailModel>();
    }
}
