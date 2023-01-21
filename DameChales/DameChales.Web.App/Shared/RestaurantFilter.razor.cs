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

		public class FormModel
		{
			public string NameFilter = string.Empty;
			public string AddressFilter = string.Empty;
		}

		public FormModel Model { get; set; } = new FormModel();	

		public async Task Filter()
		{
			if (Model.NameFilter == string.Empty && Model.AddressFilter == string.Empty)
			{
				RestaurantList = await RestaurantFacade.GetAllAsync();
			}
			if (Model.NameFilter != string.Empty && Model.AddressFilter == string.Empty)
			{
				RestaurantList = await RestaurantFacade.GetByNameAsync(Model.NameFilter);
			}
			if (Model.NameFilter == string.Empty && Model.AddressFilter != string.Empty)
			{
				RestaurantList =  await RestaurantFacade.GetByAddressAsync(Model.AddressFilter);
			}
			if (Model.NameFilter != string.Empty && Model.AddressFilter != string.Empty)
			{
				var a = await RestaurantFacade.GetByNameAsync(Model.NameFilter);
				var b = await RestaurantFacade.GetByAddressAsync(Model.AddressFilter);
				RestaurantList = a.Intersect(b).ToList();
			}
			await SearchCallback.InvokeAsync();
			return; 
			
		}
	}

}
