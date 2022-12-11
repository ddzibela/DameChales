using DameChales.Common.Models;

namespace DameChales.Web.DAL.Repositories
{
    public class RestaurantRepository : RepositoryBase<RestaurantDetailModel>
    {
        public override string TableName { get; } = "restaurants";

        public RestaurantRepository(LocalDb localDb)
            : base(localDb)
        {
        }
    }
}