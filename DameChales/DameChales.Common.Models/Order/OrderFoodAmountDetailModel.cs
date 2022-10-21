using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DameChales.Common.Models
{
    public record OrderFoodAmountDetailModel : IWithId
    {
        public required Guid Id { get; init; }

        [Range(0, int.MaxValue)]
        public required int Amount { get; set; }
        public string? Note { get; set; }
        public required FoodListModel Food { get; set; }
    }
}
