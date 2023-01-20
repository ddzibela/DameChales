using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using DameChales.Common.Enums;
using DameChales.Common.Extensions;
using DameChales.Common.Models;
using DameChales.Web.BL.Facades;
using Microsoft.AspNetCore.Components;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;
using DameChales.Web.App.Shared;

namespace DameChales.Web.App.Pages
{
    public partial class SearchPage
    {
        [Inject]
        private FoodFacade FoodFacade { get; set; } = null!;
        [Inject]
        private RestaurantFacade RestaurantFacade { get; set; } = null!;
        private ICollection<FoodListModel> Foods { get; set; } = new List<FoodListModel>();
        private ICollection<RestaurantListModel> Restaurants { get; set; } = new List<RestaurantListModel>();
        private HashSet<Alergens> Alergens { get; set; } = new HashSet<Alergens> { };

        private FoodFilter? FoodFilter { get; set; } = null;
        //filter strings
        private string FoodNameFilter { get; set; } = string.Empty;
        private string FoodDescriptionFilter { get; set; } = string.Empty;
        private string RestaurantNameFilter { get; set; } = string.Empty;
        private string RestaurantAddressFilter { get; set; } = string.Empty;
        //order by flags

        protected override async Task OnInitializedAsync()
        {
            FoodFilter = new FoodFilter(FoodFacade);
            await base.OnInitializedAsync();
        }

        public async Task FilterFood()
        {
            if (FoodFilter == null) { return; }
            Foods = await FoodFilter.Filter(FoodNameFilter, FoodDescriptionFilter, Alergens.EnumSetToString());
        }
    }
}
