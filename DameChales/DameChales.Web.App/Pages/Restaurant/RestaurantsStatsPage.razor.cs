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

		private List<OrderListModel> orderListModel { get; set; } = new List<OrderListModel>();

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
			orderListModel = await orderFacade.GetByRestaurantIdAsync(Id);

			acceptedOrders = orderListModel.Where(x => x.Status == Common.Enums.OrderStatus.Accepted).Count();
            preparingOrders = orderListModel.Where(x => x.Status == Common.Enums.OrderStatus.Preparing).Count();
            deliveringOrders = orderListModel.Where(x => x.Status == Common.Enums.OrderStatus.Delivering).Count();
            deliveredOrders = orderListModel.Where(x => x.Status == Common.Enums.OrderStatus.Delivered).Count();
			totalOrders = orderListModel.Count;

			await base.OnInitializedAsync();
		}
	}
}
