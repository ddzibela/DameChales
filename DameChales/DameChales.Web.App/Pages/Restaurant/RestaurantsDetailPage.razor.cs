using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DameChales.Common.Models;
using DameChales.Web.BL.Facades;
using Microsoft.AspNetCore.Components;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DameChales.Web.App.Pages
{
    public partial class RestaurantsDetailPage
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

        private ICollection<FoodListModel> foods { get; set; } = new List<FoodListModel>();
        private OrderDetailModel orderDetailModel { get; set; } = GetNewOrderDetailModel();
        private List<OrderFoodAmountDetailModel> orderFoodModel { get; set; } = new List<OrderFoodAmountDetailModel>();
        private RestaurantDetailModel? restaurantDetailModel { get; set; }
        public string filterString { get; set; } = string.Empty;


        protected override async Task OnInitializedAsync()
        {
            if (Id == Guid.Empty)
            {
                navigationManager.NavigateTo($"/restaurants");
            }
            restaurantDetailModel = await restaurantFacade.GetByIdAsync( Id );
            orderDetailModel.RestaurantGuid = Id;
            foods = await foodFacade.GetByRestaurantIdAsync( Id );

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
            };
    
        public async Task filter()
        {
            if (filterString == string.Empty)
            {
                foods = await foodFacade.GetByRestaurantIdAsync(Id);
                return;
            }
            foods.Clear();

            foods = (ICollection<FoodListModel>)foodFacade.GetByNameAsync(filterString).Result.Where(x => x.Id == Id);
        }

        public void AddToOrder(FoodListModel food)
        {
            var orderAmount = new OrderFoodAmountDetailModel
            {
                Id = Guid.NewGuid(),
                Amount = 1,
                Note = string.Empty,
                Food = food,
            };

            orderFoodModel.Add(orderAmount);
            orderDetailModel.FoodAmounts = orderFoodModel;
        }
    }
}
