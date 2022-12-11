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
        private OrderFacade orderFacade { get; set; }
        [Inject]
        private NavigationManager navigationManager { get; set; } = null!;
        [Parameter]
        public Guid Id { get; set; }

        public OrderDetailModel orderModel { get; set; }
        
        protected override async Task OnInitializedAsync()
        {
            if (Id == Guid.Empty)
            {
                navigationManager.NavigateTo("/restaurants");
            }
            orderModel = await orderFacade.GetByIdAsync(Id);
            
            await base.OnInitializedAsync();
        }
    }
}
