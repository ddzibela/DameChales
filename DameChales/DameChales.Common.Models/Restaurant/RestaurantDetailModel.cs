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
        public required string Name { get; init; }
        public required string? Description { get; set; }
        public required string? PhotoURL { get; init; }
        public required string Address { get; init; }
        public required string GPSCoordinates { get; init; }

        public IList<FoodListModel> Foods {get; init; } = new List<FoodListModel>();
        public IList<OrderListModel> Orders { get; init; } = new List<OrderListModel>();
    }
}
