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
using DameChales.Common;

namespace DameChales.Web.App.Pages
{
    public partial class SearchPage
    {
        private RestaurantFilter? RestaurantFilter { get; set; } = null;
        private FoodFilter? FoodFilter { get; set; } = null;
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }
    }
}
