using DameChales.API.DAL.Common.Entities;
using DameChales.Common.Enums;

namespace DameChales.API.DAL.Common.Repositories
{
    public interface IOrderRepository : IApiRepository<OrderEntity>
    {
        IList<OrderEntity> GetByRestaurantId(Guid id);
        IList<OrderEntity> GetByFoodId(Guid id);
        IList<OrderEntity> GetByStatus(Guid restaurantId, OrderStatus status);
    }
}
