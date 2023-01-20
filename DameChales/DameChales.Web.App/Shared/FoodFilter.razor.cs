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
        private string NameRegex = string.Empty;
        private string DescRegex = string.Empty;
        private string AlergensString = string.Empty;
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
            AlergensString = Alergens.EnumSetToString();
            var ret = new List<FoodListModel>();
            var name = new List<FoodListModel>();
            var desc = new List<FoodListModel>();
            var alergens = new List<FoodListModel>();

            if (NameRegex == string.Empty && DescRegex == string.Empty && AlergensString == string.Empty)
            {
                Foods = await FoodFacade.GetAllAsync();
                await SearchCallback.InvokeAsync();
                return;
            }
            if (NameRegex == string.Empty && DescRegex == string.Empty && AlergensString != string.Empty)
            {
                Foods = await FoodFacade.GetWithoutAlergensAsync(AlergensString);
                await SearchCallback.InvokeAsync();
                return;
            }
            if (NameRegex == string.Empty && DescRegex != string.Empty && AlergensString != string.Empty)
            {
                desc = await FoodFacade.GetByDescAsync(DescRegex);
                alergens = await FoodFacade.GetWithoutAlergensAsync(AlergensString);
                ret = desc.Intersect(alergens).ToList();
                Foods = ret;
                await SearchCallback.InvokeAsync();
                return;
            }
            if (NameRegex != string.Empty && DescRegex != string.Empty && AlergensString != string.Empty)
            {
                name = await FoodFacade.GetByNameAsync(NameRegex);
                desc = await FoodFacade.GetByDescAsync(DescRegex);
                alergens = await FoodFacade.GetWithoutAlergensAsync(AlergensString);
                ret = desc.Intersect(alergens).Intersect(name).ToList();
                Foods = ret;
                await SearchCallback.InvokeAsync();
                return;
            }
            if (NameRegex != string.Empty && DescRegex != string.Empty && AlergensString == string.Empty)
            {
                name = await FoodFacade.GetByNameAsync(NameRegex);
                desc = await FoodFacade.GetByDescAsync(DescRegex);
                ret = desc.Intersect(name).ToList();
                Foods = ret;
                await SearchCallback.InvokeAsync();
                return;
            }
            if (NameRegex != string.Empty && DescRegex == string.Empty && AlergensString != string.Empty)
            {
                name = await FoodFacade.GetByNameAsync(NameRegex);
                alergens = await FoodFacade.GetWithoutAlergensAsync(AlergensString);
                ret = alergens.Intersect(name).ToList();
                Foods = ret;
                await SearchCallback.InvokeAsync();
                return;
            }
            if (NameRegex != string.Empty && DescRegex == string.Empty && AlergensString == string.Empty)
            {
                Foods = await FoodFacade.GetByNameAsync(NameRegex);
                await SearchCallback.InvokeAsync();
                return;
            }
            if (NameRegex == string.Empty && DescRegex != string.Empty && AlergensString == string.Empty)
            {
                Foods = await FoodFacade.GetByDescAsync(NameRegex);
                await SearchCallback.InvokeAsync();
                return;
            }
            await SearchCallback.InvokeAsync();
            Foods = ret;
        }

        public async Task FilterId()
        {
            Foods.Clear();
            AlergensString = Alergens.EnumSetToString();
            var ret = new List<FoodListModel>();
            var name = new List<FoodListModel>();
            var desc = new List<FoodListModel>();
            var alergens = new List<FoodListModel>();

            if (NameRegex == string.Empty && DescRegex == string.Empty && AlergensString == string.Empty)
            {
                Foods = await FoodFacade.GetAllAsync();
                await SearchCallback.InvokeAsync();
                return;
            }
            if (NameRegex == string.Empty && DescRegex == string.Empty && AlergensString != string.Empty)
            {
                Foods = await FoodFacade.GetWithoutAlergensAsync(Id, AlergensString);
                await SearchCallback.InvokeAsync();
                return;
            }
            if (NameRegex == string.Empty && DescRegex != string.Empty && AlergensString != string.Empty)
            {
                desc = await FoodFacade.GetByDescAsync(Id, DescRegex);
                alergens = await FoodFacade.GetWithoutAlergensAsync(Id, AlergensString);
                ret = desc.Intersect(alergens).ToList();
                Foods = ret;
                await SearchCallback.InvokeAsync();
                return;
            }
            if (NameRegex != string.Empty && DescRegex != string.Empty && AlergensString != string.Empty)
            {
                name = await FoodFacade.GetByNameAsync(Id, NameRegex);
                desc = await FoodFacade.GetByDescAsync(Id, DescRegex);
                alergens = await FoodFacade.GetWithoutAlergensAsync(Id, AlergensString);
                ret = desc.Intersect(alergens).Intersect(name).ToList();
                Foods = ret;
                await SearchCallback.InvokeAsync();
                return;
            }
            if (NameRegex != string.Empty && DescRegex != string.Empty && AlergensString == string.Empty)
            {
                name = await FoodFacade.GetByNameAsync(Id, NameRegex);
                desc = await FoodFacade.GetByDescAsync(Id, DescRegex);
                ret = desc.Intersect(name).ToList();
                Foods = ret;
                await SearchCallback.InvokeAsync();
                return;
            }
            if (NameRegex != string.Empty && DescRegex == string.Empty && AlergensString != string.Empty)
            {
                name = await FoodFacade.GetByNameAsync(Id, NameRegex);
                alergens = await FoodFacade.GetWithoutAlergensAsync(Id, AlergensString);
                ret = alergens.Intersect(name).ToList();
                Foods = ret;
                await SearchCallback.InvokeAsync();
                return;
            }
            if (NameRegex != string.Empty && DescRegex == string.Empty && AlergensString == string.Empty)
            {
                Foods = await FoodFacade.GetByNameAsync(Id, NameRegex);
                await SearchCallback.InvokeAsync();
                return;
            }
            if (NameRegex == string.Empty && DescRegex != string.Empty && AlergensString == string.Empty)
            {
                Foods = await FoodFacade.GetByDescAsync(Id, NameRegex);
                await SearchCallback.InvokeAsync();
                return;
            }
            Foods = ret;
        }
    }
}
