using System.Collections.Generic;
using System.Threading.Tasks;
using DameChales.Common.Models;
using DameChales.Web.BL.Facades;
using Microsoft.AspNetCore.Components;

namespace DameChales.Web.App.Pages
{
	public partial class RestaurantsListPage
	{
		[Inject]
		private RestaurantFacade? Facade { get; set; } = null;
		
		private IList<RestaurantListModel> RestaurantListModel = new List<RestaurantListModel>();

		public string FilterString { get; set; } = string.Empty;

		protected override async Task OnInitializedAsync()
		{
			RestaurantListModel = await Facade!.GetAllAsync();
			await base.OnInitializedAsync();
		}

		public async Task filter()
		{
			if (FilterString == string.Empty)
			{
                RestaurantListModel = await Facade!.GetAllAsync();
				return;
            }

            RestaurantListModel = await Facade.GetByNameAsync(FilterString);
			RestaurantListModel = (IList<RestaurantListModel>)RestaurantListModel.Concat(await Facade.GetByAddressAsync(FilterString));
		}

	}
}
