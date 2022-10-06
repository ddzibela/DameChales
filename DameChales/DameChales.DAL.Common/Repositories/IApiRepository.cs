using System;
using System.Collections.Generic;
using DameChales.API.DAL.Common.Entities.Interfaces;
using DameChales.API.DAL.Common.Entities;

namespace DameChales.API.DAL.Common.Repositories
{
    public interface IApiRepository<TEntity>
        where TEntity : IEntity
    {
        IList<TEntity> GetAll();
        TEntity? GetById(Guid id);
        Guid Insert(TEntity entity);
        Guid? Update(TEntity entity);
        void Remove(Guid id);
        bool Exists(Guid id);
    }
}
