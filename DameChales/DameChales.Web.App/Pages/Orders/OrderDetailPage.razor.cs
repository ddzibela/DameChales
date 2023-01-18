using System.Collections.Generic;
using System.Threading.Tasks;
using DameChales.Common.Models;
using DameChales.Web.BL.Facades;
using Microsoft.AspNetCore.Components;

namespace DameChales.Web.App.Pages
{
    public partial class OrderDetailPage
    {
        [Inject]
        private OrderFacade OrderFacade { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;
        [Parameter]
        public Guid Id { get; set; }

        public OrderDetailModel? OrderModel { get; set; }
        
        protected override async Task OnInitializedAsync()
        {
            if (Id == Guid.Empty)
            {
                NavigationManager.NavigateTo("/restaurants");
            }
            OrderModel = await OrderFacade.GetByIdAsync(Id);
            
            await base.OnInitializedAsync();
        }
    }
}
