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
    }
}
