using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using DameChales.Common.Enums;
using DameChales.Common.Extensions;
using DameChales.Common.Models;
using DameChales.Web.BL.Facades;
using Microsoft.AspNetCore.Components;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;
using DameChales.Web.App.Shared;

namespace DameChales.Web.App.Pages
{
    public partial class RestaurantsDetailPage
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

        private FoodFilter? FoodFilter { get; set; }

        private ICollection<FoodListModel> Foods { get; set; } = new List<FoodListModel>();
        private OrderDetailModel OrderDetailModel { get; set; } = GetNewOrderDetailModel();
        private RestaurantDetailModel? RestaurantDetailModel { get; set; }
        public string FilterString { get; set; } = string.Empty;
        private bool OrderByNameFlag { get; set; } = false;
        private bool OrderByPriceFlag { get; set; } = false;
        private HashSet<Alergens> Alergens { get; set; } = new HashSet<Alergens>();


        protected override async Task OnInitializedAsync()
        {

            if (Id == Guid.Empty)
            {
                NavigationManager.NavigateTo($"/restaurants");
            }
            RestaurantDetailModel = await RestaurantFacade.GetByIdAsync( Id );
            OrderDetailModel.RestaurantGuid = Id;
            Foods = await FoodFacade.GetByRestaurantIdAsync( Id );

            await base.OnInitializedAsync();
        }

        private static OrderDetailModel GetNewOrderDetailModel()
            => new()
            {
                Id = Guid.NewGuid(),
                Name = string.Empty,
                DeliveryTime = DateTime.Now,
                Note = string.Empty,
                Status = Common.Enums.OrderStatus.Accepted,
                RestaurantGuid = Guid.Empty,
                FoodAmounts = new List<OrderFoodAmountDetailModel>(),
                Address = string.Empty
            };
    
        public void Filter()
        {
            Foods.Clear();
            Foods = FoodFilter.Foods;
        }
    
        public void AddToOrder(FoodListModel food)
        {
            if (OrderDetailModel.FoodAmounts.Where(x => x.Food.Id == food.Id).Count() > 0)
            {
                return;
            }
            var orderAmount = new OrderFoodAmountDetailModel
            {
                Id = Guid.NewGuid(),
                Amount = 1,
                Note = string.Empty,
                Food = food,
            };

            OrderDetailModel.FoodAmounts.Add(orderAmount);
        }

        public void RemoveFromOrder(FoodListModel food)
        {
            var itemToRemove = OrderDetailModel.FoodAmounts.SingleOrDefault(x => x.Food.Id == food.Id);
            if (itemToRemove != null)
            {
                OrderDetailModel.FoodAmounts.Remove(itemToRemove);
            }
        }
    
        public async Task PlaceOrder()
        {
            if (OrderDetailModel.Name != "" && OrderDetailModel.Address != "")
            {
                await OrderFacade.SaveAsync(OrderDetailModel);
                await OrderFacade.SaveAsync(OrderDetailModel);
                OrderDetailModel = GetNewOrderDetailModel();
            }
        }

        public void OrderByPrice()
        {
            OrderByPriceFlag = !OrderByPriceFlag;
            if(OrderByPriceFlag)
            {
                Foods = Foods.OrderBy(x => x.Price).ToList();
            } else
            {
                Foods = Foods.OrderByDescending(x => x.Price).ToList();
            }
        }

        public void OrderByName()
        {
            OrderByNameFlag = !OrderByNameFlag;
            if(OrderByNameFlag)
            {
                Foods = Foods.OrderBy(x =>x.Name).ToList();
            } else
            {
                Foods = Foods.OrderByDescending(x =>x.Name).ToList();
            }
        }
    }
}
