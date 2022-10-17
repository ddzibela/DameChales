using DameChales.API.DAL.Common.Entities.Interfaces;
using System;

namespace DameChales.API.DAL.Common.Entities
{
    public abstract record EntityBase : IEntity
    {
        public required Guid Id { get; init; }

        public EntityBase(Guid id)
        {
            Id = id;
        }
    }
}
