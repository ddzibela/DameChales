using DameChales.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DameChales.Common.Models
{
    public record OrderDetailModel : IWithId
    {
        public required Guid Id { get; init; }

        public Guid RestaurantGuid { get; set; }
        public required string Name {get; set;}
        public required string? Note { get; set;}
        public required TimeSpan DeliveryTime { get; set; }
        public required OrderStatus Status { get; set; }
        public IList<OrderFoodAmountDetailModel> FoodAmounts { get; set; } = new List<OrderFoodAmountDetailModel>();
    }
}
