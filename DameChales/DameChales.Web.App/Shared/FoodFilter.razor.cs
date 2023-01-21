using DameChales.Common.Enums;
using DameChales.Common.Extensions;
using DameChales.Common.Models;
using DameChales.Web.BL.Facades;
using Microsoft.AspNetCore.Components;

namespace DameChales.Web.App.Shared
{
    public partial class FoodFilter
    {
        [Inject]
        private FoodFacade FoodFacade { get; set; }
        [Parameter]
        public EventCallback SearchCallback { get; set; }
        [Parameter]
        public Guid Id { get; set; }

        public class FormModel
        {
            public string NameRegex = string.Empty;
            public string DescRegex = string.Empty;
            public string AlergensString = string.Empty;
        }
        public FormModel Model { get; set; } = new FormModel();
        private HashSet<Alergens> Alergens { get; set; } = new HashSet<Alergens> { };

		public IList<FoodListModel> Foods { get; private set; } = new List<FoodListModel>();

        public async Task Filter()
        {
            if(Id == Guid.Empty)
            {
                await FilterNoId();
            } else
            {
                await FilterId();
            }
            return;
        }

        public async Task FilterNoId()
        {
            Foods.Clear();
            Model.AlergensString = Alergens.EnumSetToString();
            var ret = new List<FoodListModel>();
            var name = new List<FoodListModel>();
            var desc = new List<FoodListModel>();
            var alergens = new List<FoodListModel>();

            if (Model.NameRegex == string.Empty && Model.DescRegex == string.Empty && Model.AlergensString == string.Empty)
            {
                Foods = await FoodFacade.GetAllAsync();
                await SearchCallback.InvokeAsync();
                return;
            }
            if (Model.NameRegex == string.Empty && Model.DescRegex == string.Empty && Model.AlergensString != string.Empty)
            {
                Foods = await FoodFacade.GetWithoutAlergensAsync(Model.AlergensString);
                await SearchCallback.InvokeAsync();
                return;
            }
            if (Model.NameRegex == string.Empty && Model.DescRegex != string.Empty && Model.AlergensString != string.Empty)
            {
                desc = await FoodFacade.GetByDescAsync(Model.DescRegex);
                alergens = await FoodFacade.GetWithoutAlergensAsync(Model.AlergensString);
                ret = desc.Intersect(alergens).ToList();
                Foods = ret;
                await SearchCallback.InvokeAsync();
                return;
            }
            if (Model.NameRegex != string.Empty && Model.DescRegex != string.Empty && Model.AlergensString != string.Empty)
            {
                name = await FoodFacade.GetByNameAsync(Model.NameRegex);
                desc = await FoodFacade.GetByDescAsync(Model.DescRegex);
                alergens = await FoodFacade.GetWithoutAlergensAsync(Model.AlergensString);
                ret = desc.Intersect(alergens).Intersect(name).ToList();
                Foods = ret;
                await SearchCallback.InvokeAsync();
                return;
            }
            if (Model.NameRegex != string.Empty && Model.DescRegex != string.Empty && Model.AlergensString == string.Empty)
            {
                name = await FoodFacade.GetByNameAsync(Model.NameRegex);
                desc = await FoodFacade.GetByDescAsync(Model.DescRegex);
                ret = desc.Intersect(name).ToList();
                Foods = ret;
                await SearchCallback.InvokeAsync();
                return;
            }
            if (Model.NameRegex != string.Empty && Model.DescRegex == string.Empty && Model.AlergensString != string.Empty)
            {
                name = await FoodFacade.GetByNameAsync(Model.NameRegex);
                alergens = await FoodFacade.GetWithoutAlergensAsync(Model.AlergensString);
                ret = alergens.Intersect(name).ToList();
                Foods = ret;
                await SearchCallback.InvokeAsync();
                return;
            }
            if (Model.NameRegex != string.Empty && Model.DescRegex == string.Empty && Model.AlergensString == string.Empty)
            {
                Foods = await FoodFacade.GetByNameAsync(Model.NameRegex);
                await SearchCallback.InvokeAsync();
                return;
            }
            if (Model.NameRegex == string.Empty && Model.DescRegex != string.Empty && Model.AlergensString == string.Empty)
            {
                Foods = await FoodFacade.GetByDescAsync(Model.DescRegex);
                await SearchCallback.InvokeAsync();
                return;
            }
            await SearchCallback.InvokeAsync();
            Foods = ret;
        }

        public async Task FilterId()
        {
            Foods.Clear();
            Model.AlergensString = Alergens.EnumSetToString();
            var ret = new List<FoodListModel>();
            var name = new List<FoodListModel>();
            var desc = new List<FoodListModel>();
            var alergens = new List<FoodListModel>();

            if (Model.NameRegex == string.Empty && Model.DescRegex == string.Empty && Model.AlergensString == string.Empty)
            {
                Foods = await FoodFacade.GetAllAsync();
                await SearchCallback.InvokeAsync();
                return;
            }
            if (Model.NameRegex == string.Empty && Model.DescRegex == string.Empty && Model.AlergensString != string.Empty)
            {
                Foods = await FoodFacade.GetWithoutAlergensAsync(Id, Model.AlergensString);
                await SearchCallback.InvokeAsync();
                return;
            }
            if (Model.NameRegex == string.Empty && Model.DescRegex != string.Empty && Model.AlergensString != string.Empty)
            {
                desc = await FoodFacade.GetByDescAsync(Id, Model.DescRegex);
                alergens = await FoodFacade.GetWithoutAlergensAsync(Id, Model.AlergensString);
                ret = desc.Intersect(alergens).ToList();
                Foods = ret;
                await SearchCallback.InvokeAsync();
                return;
            }
            if (Model.NameRegex != string.Empty && Model.DescRegex != string.Empty && Model.AlergensString != string.Empty)
            {
                name = await FoodFacade.GetByNameAsync(Id, Model.NameRegex);
                desc = await FoodFacade.GetByDescAsync(Id, Model.DescRegex);
                alergens = await FoodFacade.GetWithoutAlergensAsync(Id, Model.AlergensString);
                ret = desc.Intersect(alergens).Intersect(name).ToList();
                Foods = ret;
                await SearchCallback.InvokeAsync();
                return;
            }
            if (Model.NameRegex != string.Empty && Model.DescRegex != string.Empty && Model.AlergensString == string.Empty)
            {
                name = await FoodFacade.GetByNameAsync(Id, Model.NameRegex);
                desc = await FoodFacade.GetByDescAsync(Id, Model.DescRegex);
                ret = desc.Intersect(name).ToList();
                Foods = ret;
                await SearchCallback.InvokeAsync();
                return;
            }
            if (Model.NameRegex != string.Empty && Model.DescRegex == string.Empty && Model.AlergensString != string.Empty)
            {
                name = await FoodFacade.GetByNameAsync(Id, Model.NameRegex);
                alergens = await FoodFacade.GetWithoutAlergensAsync(Id, Model.AlergensString);
                ret = alergens.Intersect(name).ToList();
                Foods = ret;
                await SearchCallback.InvokeAsync();
                return;
            }
            if (Model.NameRegex != string.Empty && Model.DescRegex == string.Empty && Model.AlergensString == string.Empty)
            {
                Foods = await FoodFacade.GetByNameAsync(Id, Model.NameRegex);
                await SearchCallback.InvokeAsync();
                return;
            }
            if (Model.NameRegex == string.Empty && Model.DescRegex != string.Empty && Model.AlergensString == string.Empty)
            {
                Foods = await FoodFacade.GetByDescAsync(Id, Model.DescRegex);
                await SearchCallback.InvokeAsync();
                return;
            }
            Foods = ret;
        }
    }
}
