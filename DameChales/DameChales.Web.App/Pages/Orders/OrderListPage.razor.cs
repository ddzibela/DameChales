using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper.Execution;
using DameChales.Common.Enums;
using DameChales.Common.Models;
using DameChales.Web.BL.Facades;
using DameChales.Web.DAL.Repositories;
using Microsoft.AspNetCore.Components;

namespace DameChales.Web.App.Pages
{
    public partial class OrderListPage
    {
        [Inject]
        private OrderFacade OrderFacade { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;
        [Parameter]
        public Guid Id { get; set; }

        private IList<OrderListModel> AllOrders { get; set; } = new List<OrderListModel>();
        private IList<OrderListModel> AcceptedOrders { get; set; } = new List<OrderListModel>();
        private IList<OrderListModel> PreparingOrders { get; set; } = new List<OrderListModel>();
        private IList<OrderListModel> DeliveringOrders { get; set; } = new List<OrderListModel>();
        private IList<OrderListModel> DeliveredOrders { get; set; } = new List<OrderListModel>();

        private OrderDetailModel Tmp { get; set; } = GetNewOrderDetailModel();
        private IList<OrderDetailModel> AcceptedOrdersDetail { get; set; } = new List<OrderDetailModel>();
        private IList<OrderDetailModel> PreparingOrdersDetail { get; set; } = new List<OrderDetailModel>();

        private IList<OrderDetailModel> DeliveringOrdersDetail { get; set; } = new List<OrderDetailModel>();
        private IList<OrderDetailModel> DeliveredOrdersDetail { get; set; } = new List<OrderDetailModel>();

        protected override async Task OnInitializedAsync()
        {
            if (Id == Guid.Empty)
            {
                NavigationManager.NavigateTo("/restaurants");
            }
            AllOrders = await OrderFacade.GetByRestaurantIdAsync(Id);
            
            AcceptedOrders = AllOrders.Where(x => x.Status == Common.Enums.OrderStatus.Accepted).ToList();
            PreparingOrders = AllOrders.Where(x => x.Status == Common.Enums.OrderStatus.Preparing).ToList();
            DeliveringOrders = AllOrders.Where(x => x.Status == Common.Enums.OrderStatus.Delivering).ToList();
            DeliveredOrders = AllOrders.Where(x => x.Status == Common.Enums.OrderStatus.Delivered).ToList();

            foreach (var order in AcceptedOrders)
            {
                Tmp = await OrderFacade.GetByIdAsync(order.Id);
                AcceptedOrdersDetail.Add(Tmp);
            }

            foreach (var order in PreparingOrders)
            {
                Tmp = await OrderFacade.GetByIdAsync(order.Id);
                PreparingOrdersDetail.Add(Tmp);
            }

            foreach (var order in DeliveringOrders)
            {
                Tmp = await OrderFacade.GetByIdAsync(order.Id);
                DeliveringOrdersDetail.Add(Tmp);
            }

            foreach (var order in DeliveredOrders)
            {
                Tmp = await OrderFacade.GetByIdAsync(order.Id);
                DeliveredOrdersDetail.Add(Tmp);
            }

            await base.OnInitializedAsync();
        }

        public async Task UpdateOrderStatus(OrderDetailModel order)
        {
            await OrderFacade.SaveAsync(order);
            NavigationManager.NavigateTo($"/restaurants/orders/{order.RestaurantGuid}", true);
        }

        private static OrderDetailModel GetNewOrderDetailModel()
            => new()
            {
                Id = Guid.NewGuid(),
                RestaurantGuid = Guid.NewGuid(),
                Name = string.Empty,
                Note = string.Empty,
                DeliveryTime = DateTime.MinValue,
                Status = Common.Enums.OrderStatus.Accepted,
                Address = string.Empty
            };
    }
}
