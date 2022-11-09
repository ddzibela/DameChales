using DameChales.Common.Models;

namespace DameChales.Web.DAL.Repositories
{
    public class OrderRepository : RepositoryBase<OrderDetailModel>
    {
        public override string TableName { get; } = "orders";

        public OrderRepository(LocalDb localDb)
            : base(localDb)
        {
        }
    }
}