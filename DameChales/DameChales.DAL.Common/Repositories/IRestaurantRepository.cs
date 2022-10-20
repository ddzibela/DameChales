using DameChales.API.DAL.Common.Entities;

namespace DameChales.API.DAL.Common.Repositories
{
    public interface IRestaurantRepository : IApiRepository<RestaurantEntity>
    {
        IList<RestaurantEntity> GetByFoodId(Guid id);
        RestaurantEntity? GetByName(string name);
        RestaurantEntity? GetByAddress(string address);
    }
}
