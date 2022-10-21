using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DameChales.Common;

namespace DameChales.Common.Models
{
    public record FoodListModel : IWithId
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
        public required string? PhotoURL { get; init; }
        public required double Price { get; init; }
    }
}
