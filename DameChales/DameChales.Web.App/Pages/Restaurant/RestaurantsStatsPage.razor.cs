using System.Collections.Generic;
using System.Threading.Tasks;
using DameChales.Common.Models;
using DameChales.Web.BL.Facades;
using Microsoft.AspNetCore.Components;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DameChales.Web.App.Pages
{

	public partial class RestaurantsStatsPage
    {
		[Inject]
		private NavigationManager navigationManager { get; set; } = null!;
		[Inject]
		private FoodFacade foodFacade { get; set; } = null!;
		[Inject]
		private RestaurantFacade restaurantFacade { get; set; } = null!;
		[Inject]
		private OrderFacade orderFacade { get; set; } = null!;
		[Parameter]
		public Guid Id { get; set; } = Guid.Empty;

		public double profits { get; set; }
		public int totalOrders { get; set; }
		public int acceptedOrders { get; set; }
		public int preparingOrders { get; set; }
		public int deliveringOrders { get; set; }
		public int deliveredOrders { get; set; }

		protected override async Task OnInitializedAsync()
		{
			if (Id == Guid.Empty)
			{
				navigationManager.NavigateTo($"/restaurants");
			}
			profits = await restaurantFacade.GetEarningsAsync(Id);
			//Can't do this asynchronously sadly
			acceptedOrders = orderFacade.GetByStatusAsync(Id, Common.Enums.OrderStatus.Accepted).Result.ToList().Count();
			preparingOrders = orderFacade.GetByStatusAsync(Id, Common.Enums.OrderStatus.Preparing).Result.ToList().Count();
			deliveringOrders = orderFacade.GetByStatusAsync(Id, Common.Enums.OrderStatus.Delivering).Result.ToList().Count();
			deliveredOrders = orderFacade.GetByStatusAsync(Id, Common.Enums.OrderStatus.Delivered).Result.ToList().Count();
			totalOrders = acceptedOrders + deliveredOrders + deliveringOrders + preparingOrders;

			await base.OnInitializedAsync();
		}
	}
}
