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
			public string DescriptionFilter = string.Empty;
		}

		public FormModel Model { get; set; } = new FormModel();	

		public async Task Filter()
		{
			if (Model.NameFilter == string.Empty && Model.AddressFilter == string.Empty && Model.DescriptionFilter == string.Empty)
			{
				RestaurantList = await RestaurantFacade.GetAllAsync();
			}

            if (Model.NameFilter == string.Empty && Model.AddressFilter != string.Empty && Model.DescriptionFilter == string.Empty)
            {
                RestaurantList = await RestaurantFacade.GetByAddressAsync(Model.AddressFilter);
            }

			if (Model.NameFilter == string.Empty && Model.AddressFilter == string.Empty && Model.DescriptionFilter != string.Empty)
			{
				RestaurantList = await RestaurantFacade.GetByDescAsync(Model.DescriptionFilter);
            }

            if (Model.NameFilter == string.Empty && Model.AddressFilter != string.Empty && Model.DescriptionFilter != string.Empty)
            {
                var a = await RestaurantFacade.GetByAddressAsync(Model.AddressFilter);
                var b = await RestaurantFacade.GetByDescAsync(Model.DescriptionFilter);
                RestaurantList = a.Intersect(b).ToList();
            }


            if (Model.NameFilter != string.Empty && Model.AddressFilter == string.Empty && Model.DescriptionFilter == string.Empty)
			{
				RestaurantList = await RestaurantFacade.GetByNameAsync(Model.NameFilter);
			}

            if (Model.NameFilter != string.Empty && Model.AddressFilter == string.Empty && Model.DescriptionFilter != string.Empty)
            {
                var a = await RestaurantFacade.GetByNameAsync(Model.NameFilter);
                var b = await RestaurantFacade.GetByDescAsync(Model.DescriptionFilter);
                RestaurantList = a.Intersect(b).ToList();
            }

            if (Model.NameFilter != string.Empty && Model.AddressFilter != string.Empty && Model.DescriptionFilter == string.Empty)
			{
				var a = await RestaurantFacade.GetByNameAsync(Model.NameFilter);
				var b = await RestaurantFacade.GetByAddressAsync(Model.AddressFilter);
				RestaurantList = a.Intersect(b).ToList();
			}

            if (Model.NameFilter != string.Empty && Model.AddressFilter != string.Empty && Model.DescriptionFilter != string.Empty)
            {
                var name = await RestaurantFacade.GetByNameAsync(Model.NameFilter);
                var address = await RestaurantFacade.GetByAddressAsync(Model.AddressFilter);
                var desc = await RestaurantFacade.GetByDescAsync(Model.DescriptionFilter);
                RestaurantList = desc.Intersect(name).Intersect(address).ToList();
            }

            await SearchCallback.InvokeAsync();
			return; 
			
		}
	}

}
