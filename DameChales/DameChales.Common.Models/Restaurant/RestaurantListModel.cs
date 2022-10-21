using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DameChales.Common.Models
{
    public record RestaurantListModel : IWithId
    {
        public required Guid Id { get; init; }

        public required string Name { get; init; }
        public required string? Description { get; set; }
        public required string? PhotoURL { get; init; }

    }
}
