using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using DameChales.Common.Enums;
using DameChales.Common;

namespace DameChales.Web.DAL.Repositories
{
    public abstract class RepositoryBase<T> : IWebRepository<T>
        where T : IWithId
    {
        private readonly LocalDb localDb;
        public abstract string TableName { get; }

        protected RepositoryBase(LocalDb localDb)
        {
            this.localDb = localDb;
        }

        public async Task<IList<T>> GetAllAsync()
        {
            return await localDb.GetAllAsync<T>(TableName);
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await localDb.GetByIdAsync<T>(TableName, id);
        }
        public async Task<T> GetEarningsAsync(Guid id)
        {
            return await localDb.GetEarningsAsync<T>(TableName, id);
        }

        public async Task<T> GetByFoodIdAsync(Guid id)
        {
            return await localDb.GetByFoodIdAsync<T>(TableName, id);
        }

        public async Task<IList<T>> GetByNameAsync(string name) {
            return await localDb.GetByNameAsync<T>(TableName, name);
        }
        public async Task<IList<T>> GetByRestaurantIdAsync(Guid id)
        {
            return await localDb.GetByRestaurantIdAsync<T>(TableName, id);
        }
        public async Task<IList<T>> GetWithoutAlergensAsync(Guid id, string alergensstr)
        {
            return await localDb.GetWithoutAlergensAsync<T>(TableName, id, alergensstr);
        }
        public async Task<IList<T>> GetByStatusAsync(Guid id, OrderStatus status)
        {
            return await localDb.GetByStatusAsync<T>(TableName, id, status);
        }
        public async Task<IList<T>> GetByAddressAsync(string address)
        {
            return await localDb.GetByAddressAsync<T>(TableName, address);
        }
        public async Task InsertAsync(T entity)
        {
            await localDb.InsertAsync(TableName, entity);
        }
        public async Task RemoveAsync(Guid id)
        {
            await localDb.RemoveAsync(TableName, id);
        }
    }
}
