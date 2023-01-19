using System.Collections.Generic;
using System.Threading.Tasks;
using DameChales.Common.Models;
using DameChales.Web.BL.Facades;
using DameChales.Common.Enums;
using DameChales.Common.Extensions;
using Microsoft.AspNetCore.Components;

namespace DameChales.Web.App.Pages
{
    public partial class FoodListPage
    {
        [Inject]
        private FoodFacade FoodFacade { get; set; } = null!;
        private RestaurantFacade RestaurantFacade { get; set; } = null!;
        private ICollection<FoodListModel> Foods { get; set; } = new List<FoodListModel>();
        [Parameter]
        public Guid Id { get; init; }
        private bool OrderByNameFlag { get; set; } = false;
        private bool OrderByPriceFlag { get; set; } = false;
        public string FilterString { get; set; } = string.Empty;
        private HashSet<Alergens> Alergens { get; set; } = new HashSet<Alergens>();

        protected override async Task OnInitializedAsync()
        {
            if (Id == Guid.Empty)
            {
                Foods = await FoodFacade.GetAllAsync();
            } else
            {
                Foods = await FoodFacade.GetByRestaurantIdAsync(Id);
            }

            await base.OnInitializedAsync();
        }

        public async Task Filter()
        {
            Foods = await FoodFacade.GetAllAsync();
            var AlergensStr = Alergens.EnumSetToString();
            IList<FoodListModel> filtered = new List<FoodListModel>();
            foreach (var food in Foods)
            {
                var ret = await FoodFacade.GetWithoutAlergensAsync(food.RestaurantGuid, AlergensStr);
                foreach (var f in ret)
                {
                    if (!filtered.Any(s => s == f))
                    {
                        filtered.Add(f);
                    }
                }
            }

            Foods = filtered;
        }

        public void OrderByPrice()
        {
            OrderByPriceFlag = !OrderByPriceFlag;
            if (OrderByPriceFlag)
            {
                Foods = Foods.OrderBy(x => x.Price).ToList();
            }
            else
            {
                Foods = Foods.OrderByDescending(x => x.Price).ToList();
            }
        }

        public void OrderByName()
        {
            OrderByNameFlag = !OrderByNameFlag;
            if (OrderByNameFlag)
            {
                Foods = Foods.OrderBy(x => x.Name).ToList();
            }
            else
            {
                Foods = Foods.OrderByDescending(x => x.Name).ToList();
            }
        }
    }
}
