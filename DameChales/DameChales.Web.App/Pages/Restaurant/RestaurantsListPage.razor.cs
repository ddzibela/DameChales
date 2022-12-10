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
		private RestaurantFacade? facade { get; set; } = null;
		
		private IList<RestaurantListModel> model = new List<RestaurantListModel>();

		public string filterString { get; set; } = string.Empty;

		protected override async Task OnInitializedAsync()
		{
			model = await facade!.GetAllAsync();
			await base.OnInitializedAsync();
		}

		public async Task filter()
		{
			if (filterString == string.Empty)
			{
                model = await facade!.GetAllAsync();
            }

            model = await facade.GetByNameAsync(filterString);
			model = (IList<RestaurantListModel>)model.Concat(await facade.GetByAddressAsync(filterString));
		}

	}
}
