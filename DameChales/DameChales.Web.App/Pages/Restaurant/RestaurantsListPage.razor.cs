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
		private ICollection<RestaurantListModel> model = new List<RestaurantListModel>();
		protected override async Task OnInitializedAsync()
		{
			model = await facade!.GetAllAsync();
			await base.OnInitializedAsync();
		}

	}
}
