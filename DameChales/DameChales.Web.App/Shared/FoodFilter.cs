using DameChales.Common.Models;
using DameChales.Web.BL.Facades;
using Microsoft.AspNetCore.Components;

namespace DameChales.Web.App.Shared
{
    public class FoodFilter
    {
        private FoodFacade FoodFacade { get; set; }

        public FoodFilter(FoodFacade foodFacade)
        {
            FoodFacade = foodFacade;
        }

        public async Task<List<FoodListModel>> Filter(string nameRegex, string descRegex, string alergensString)
        {
            var ret = new List<FoodListModel>();
            var name = new List<FoodListModel>();
            var desc = new List<FoodListModel>();
            var alergens = new List<FoodListModel>();

            if (nameRegex == string.Empty && descRegex == string.Empty && alergensString == string.Empty)
            {
                return await FoodFacade.GetAllAsync();
            }
            if (nameRegex == string.Empty && descRegex == string.Empty && alergensString != string.Empty)
            {
                return await FoodFacade.GetWithoutAlergensAsync(alergensString);
            }
            if (nameRegex == string.Empty && descRegex != string.Empty && alergensString != string.Empty)
            {
                desc = await FoodFacade.GetByDescAsync(descRegex);
                alergens = await FoodFacade.GetWithoutAlergensAsync(alergensString);
                ret = desc.Intersect(alergens).ToList();
                return ret;
            }
            if (nameRegex != string.Empty && descRegex != string.Empty && alergensString != string.Empty)
            {
                name = await FoodFacade.GetByNameAsync(nameRegex);
                desc = await FoodFacade.GetByDescAsync(descRegex);
                alergens = await FoodFacade.GetWithoutAlergensAsync(alergensString);
                ret = desc.Intersect(alergens).Intersect(name).ToList();
                return ret;
            }
            if (nameRegex != string.Empty && descRegex != string.Empty && alergensString == string.Empty)
            {
                name = await FoodFacade.GetByNameAsync(nameRegex);
                desc = await FoodFacade.GetByDescAsync(descRegex);
                ret = desc.Intersect(name).ToList();
                return ret;
            }
            if (nameRegex != string.Empty && descRegex == string.Empty && alergensString != string.Empty)
            {
                name = await FoodFacade.GetByNameAsync(nameRegex);
                alergens = await FoodFacade.GetWithoutAlergensAsync(alergensString);
                ret = alergens.Intersect(name).ToList();
                return ret;
            }
            if (nameRegex != string.Empty && descRegex == string.Empty && alergensString == string.Empty)
            {
                return await FoodFacade.GetByNameAsync(nameRegex);
            }
            if (nameRegex == string.Empty && descRegex != string.Empty && alergensString == string.Empty)
            {
                return await FoodFacade.GetByDescAsync(nameRegex);
            }
            return ret;
        }

        public async Task<List<FoodListModel>> Filter(Guid id, string nameRegex, string descRegex, string alergensString)
        {
            var ret = new List<FoodListModel>();
            var name = new List<FoodListModel>();
            var desc = new List<FoodListModel>();
            var alergens = new List<FoodListModel>();

            if (nameRegex == string.Empty && descRegex == string.Empty && alergensString == string.Empty)
            {
                return await FoodFacade.GetAllAsync();
            }
            if (nameRegex == string.Empty && descRegex == string.Empty && alergensString != string.Empty)
            {
                return await FoodFacade.GetWithoutAlergensAsync(id, alergensString);
            }
            if (nameRegex == string.Empty && descRegex != string.Empty && alergensString != string.Empty)
            {
                desc = await FoodFacade.GetByDescAsync(id, descRegex);
                alergens = await FoodFacade.GetWithoutAlergensAsync(id, alergensString);
                ret = desc.Intersect(alergens).ToList();
                return ret;
            }
            if (nameRegex != string.Empty && descRegex != string.Empty && alergensString != string.Empty)
            {
                name = await FoodFacade.GetByNameAsync(id, nameRegex);
                desc = await FoodFacade.GetByDescAsync(id, descRegex);
                alergens = await FoodFacade.GetWithoutAlergensAsync(id, alergensString);
                ret = desc.Intersect(alergens).Intersect(name).ToList();
                return ret;
            }
            if (nameRegex != string.Empty && descRegex != string.Empty && alergensString == string.Empty)
            {
                name = await FoodFacade.GetByNameAsync(id, nameRegex);
                desc = await FoodFacade.GetByDescAsync(id, descRegex);
                ret = desc.Intersect(name).ToList();
                return ret;
            }
            if (nameRegex != string.Empty && descRegex == string.Empty && alergensString != string.Empty)
            {
                name = await FoodFacade.GetByNameAsync(id, nameRegex);
                alergens = await FoodFacade.GetWithoutAlergensAsync(id, alergensString);
                ret = alergens.Intersect(name).ToList();
                return ret;
            }
            if (nameRegex != string.Empty && descRegex == string.Empty && alergensString == string.Empty)
            {
                return await FoodFacade.GetByNameAsync(id, nameRegex);
            }
            if (nameRegex == string.Empty && descRegex != string.Empty && alergensString == string.Empty)
            {
                return await FoodFacade.GetByDescAsync(id, nameRegex);
            }
            return ret;
        }
    }
}
