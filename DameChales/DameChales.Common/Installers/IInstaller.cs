using Microsoft.Extensions.DependencyInjection;

namespace DameChales.Common.Installers
{
    public interface IInstaller
    {
        void Install(IServiceCollection serviceCollection);
    }
}