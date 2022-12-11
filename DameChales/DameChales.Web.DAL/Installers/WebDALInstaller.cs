using DameChales.Common.Installers;
using DameChales.Web.DAL.Repositories;
using DameChales.Web.DAL;
using Microsoft.Extensions.DependencyInjection;

namespace DameChales.Web.DAL.Installers
{
    public class WebDALInstaller : IInstaller
    {
        public void Install(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<LocalDb>();
            serviceCollection.Scan(scan =>
                scan.FromAssemblyOf<WebDALInstaller>()
                    .AddClasses(classes => classes.AssignableTo(typeof(IWebRepository<>)))
                    .AsSelf()
                    .WithSingletonLifetime());
        }
    }
}
