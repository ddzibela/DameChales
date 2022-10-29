using DameChales.API.DAL.Common.Repositories;
using DameChales.API.DAL.Memory.Repositories;
using DameChales.Common.Installers;
using Microsoft.Extensions.DependencyInjection;

namespace DameChales.API.DAL.Memory.Installers
{
    public class ApiDALMemoryInstaller : IInstaller
    {
        public void Install(IServiceCollection serviceCollection)
        {
            serviceCollection.Scan(selector =>
                selector.FromAssemblyOf<ApiDALMemoryInstaller>()
                        .AddClasses(classes => classes.AssignableTo(typeof(IApiRepository<>)))
                            .AsMatchingInterface()
                            .WithTransientLifetime()
                        .AddClasses(classes => classes.AssignableTo<Storage>())
                            .AsSelf()
                            .WithSingletonLifetime()
            );
        }
    }
}
