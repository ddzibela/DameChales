using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using DameChales.Common;
using DameChales.Common.Enums;

namespace DameChales.Common.Models
{
    public record FoodDetailModel : IWithId
    {
        public required Guid Id { get; init; }

        public required string Name { get; init; }
        public required string PhotoURL { get; init; }
        public required string Description { get; init; }
        public required double Price { get; init; }

        public ICollection<Alergens> alergens { get; set; } = new HashSet<Alergens>();
    }
}
