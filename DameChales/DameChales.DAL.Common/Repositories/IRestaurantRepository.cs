using DameChales.API.DAL.Common.Entities;

namespace DameChales.API.DAL.Common.Repositories
{
    public interface IRestaurantRepository : IApiRepository<RestaurantEntity>
    {
        RestaurantEntity? GetByFoodId(Guid id);
        IList<RestaurantEntity> GetByName(string name);
        IList<RestaurantEntity> GetByDescription(string description);
        IList<RestaurantEntity> GetByAddress(string address);
        double GetEarnings(Guid id);
    }
}
