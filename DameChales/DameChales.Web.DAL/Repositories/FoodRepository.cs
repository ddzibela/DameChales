using DameChales.Common.Models;

namespace DameChales.Web.DAL.Repositories
{
    public class FoodRepository : RepositoryBase<FoodDetailModel>
    {
        public override string TableName { get; } = "foods";

        public FoodRepository(LocalDb localDb)
            : base(localDb)
        {
        }
    }
}
