using System.Collections.Generic;
using System.Threading.Tasks;
using DameChales.Common.Models;
using DameChales.Web.App.Shared;
using DameChales.Web.BL.Facades;
using Microsoft.AspNetCore.Components;

namespace DameChales.Web.App.Pages
{
	public partial class RestaurantsListPage
	{
		[Inject]
		private RestaurantFacade? Facade { get; set; } = null;
		
		private IList<RestaurantListModel> RestaurantListModel = new List<RestaurantListModel>();

		public RestaurantFilter RestaurantFilter { get; set; }

		protected override async Task OnInitializedAsync()
		{
			RestaurantListModel = await Facade!.GetAllAsync();
			await base.OnInitializedAsync();
		}

		public void Filter()
		{
			RestaurantListModel.Clear();
			RestaurantListModel = RestaurantFilter.RestaurantList;
		}

	}
}
