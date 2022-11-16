using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DameChales.Common;
using DameChales.Common.Enums;

namespace DameChales.Web.DAL.Repositories
{
    public interface IWebRepository<T>
        where T : IWithId
    {
        string TableName { get; }

        //food
        //GetByName -
        //GetByRestaurantId -
        //GetWithoutAlergens -

        //restaurant
        //GetByAddress -
        //GetByFoodId - -
        //GetByName -
        //GetEarnings - -

        //order
        //GetByStatus -
        //GetByRestaurantId -

        Task<IList<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        Task<T> GetByFoodIdAsync(Guid id);
        Task<IList<T>> GetByNameAsync(string name);
        Task<IList<T>> GetByRestaurantIdAsync(Guid id);
        Task<IList<T>> GetWithoutAlergensAsync(Guid id, string alergensstr);
        Task<T> GetEarningsAsync(Guid id);
        Task<IList<T>> GetByStatusAsync(Guid id, OrderStatus status);
        Task<IList<T>> GetByAddressAsync(string address);
        Task InsertAsync(T entity);
        Task RemoveAsync(Guid id);
    }
}
