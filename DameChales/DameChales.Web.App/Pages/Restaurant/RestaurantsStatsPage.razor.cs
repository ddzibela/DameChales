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
		private NavigationManager NavigationManager { get; set; } = null!;
		[Inject]
		private FoodFacade FoodFacade { get; set; } = null!;
		[Inject]
		private RestaurantFacade RestaurantFacade { get; set; } = null!;
		[Inject]
		private OrderFacade OrderFacade { get; set; } = null!;
		[Parameter]
		public Guid Id { get; set; } = Guid.Empty;

		private List<OrderListModel> OrderListModel { get; set; } = new List<OrderListModel>();

		public double Profits { get; set; }
		public int TotalOrders { get; set; }
		public int AcceptedOrders { get; set; }
		public int PreparingOrders { get; set; }
		public int DeliveringOrders { get; set; }
		public int DeliveredOrders { get; set; }

		protected override async Task OnInitializedAsync()
		{
			if (Id == Guid.Empty)
			{
				NavigationManager.NavigateTo($"/restaurants");
			}
			Profits = await RestaurantFacade.GetEarningsAsync(Id);
			OrderListModel = await OrderFacade.GetByRestaurantIdAsync(Id);

			AcceptedOrders = OrderListModel.Where(x => x.Status == Common.Enums.OrderStatus.Accepted).Count();
            PreparingOrders = OrderListModel.Where(x => x.Status == Common.Enums.OrderStatus.Preparing).Count();
            DeliveringOrders = OrderListModel.Where(x => x.Status == Common.Enums.OrderStatus.Delivering).Count();
            DeliveredOrders = OrderListModel.Where(x => x.Status == Common.Enums.OrderStatus.Delivered).Count();
			TotalOrders = OrderListModel.Count;

			await base.OnInitializedAsync();
		}
	}
}
