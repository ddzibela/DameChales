using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DameChales.Common.Models
{
    public record RestaurantDetailModel : IWithId
    {
        public required Guid Id { get; init; }
        public required string Name { get; set; }
        public required string? Description { get; set; }
        public required string? PhotoURL { get; set; }
        public required string Address { get; set; }
        public required string GPSCoordinates { get; set; }

        public IList<FoodListModel> Foods {get; set; } = new List<FoodListModel>();
        public IList<OrderListModel> Orders { get; set; } = new List<OrderListModel>();
    }
}
