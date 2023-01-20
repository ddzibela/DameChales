using DameChales.Common.Models;
using DameChales.Web.App.Pages;
using DameChales.Web.BL.Facades;
using Microsoft.AspNetCore.Components;

namespace DameChales.Web.App.Shared
{
	public partial class RestaurantFilter
	{
		[Inject]
		private RestaurantFacade? RestaurantFacade { get; set; } = null;

		[Parameter]
		public EventCallback SearchCallback { get; set; }
		public IList<RestaurantListModel> RestaurantList { get; private set; }
		private string NameFilter = string.Empty;
		private string AddressFilter = string.Empty;

		public async Task Filter()
		{
			if (NameFilter == string.Empty && AddressFilter == string.Empty)
			{
				RestaurantList = await RestaurantFacade.GetAllAsync();
			}
			if (NameFilter != string.Empty && AddressFilter == string.Empty)
			{
				RestaurantList = await RestaurantFacade.GetByNameAsync(NameFilter);
			}
			if (NameFilter == string.Empty && AddressFilter != string.Empty)
			{
				RestaurantList =  await RestaurantFacade.GetByAddressAsync(AddressFilter);
			}
			if (NameFilter != string.Empty && AddressFilter != string.Empty)
			{
				var a = await RestaurantFacade.GetByNameAsync(NameFilter);
				var b = await RestaurantFacade.GetByAddressAsync(AddressFilter);
				RestaurantList = a.Intersect(b).ToList();
			}
			await SearchCallback.InvokeAsync();
			return; 
			
		}
	}

}
