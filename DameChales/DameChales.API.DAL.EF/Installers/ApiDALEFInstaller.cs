using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DameChales.API.DAL.Common.Repositories;
using Microsoft.EntityFrameworkCore;


namespace DameChales.API.DAL.EF.Installers
{
    public class ApiDALEFInstaller
    {
        public void Install(IServiceCollection serviceCollection, string connectionString)
        {
            serviceCollection.AddDbContext<DameChalesDbContext>(options => options.UseSqlServer(connectionString));

            serviceCollection.Scan(selector =>
                selector.FromAssemblyOf<ApiDALEFInstaller>()
                    .AddClasses(classes => classes.AssignableTo(typeof(IApiRepository<>)))
                    .AsMatchingInterface()
                    .WithScopedLifetime());
        }
    }
}
