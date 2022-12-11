using System;
using System.Net.Http;
using DameChales.Common.BL.Facades;
using Microsoft.Extensions.DependencyInjection;

namespace DameChales.Web.BL.Installers
{
    public class WebBLInstaller
    {
        public void Install(IServiceCollection serviceCollection, string apiBaseUrl)
        {
            serviceCollection.AddTransient<IRestaurantClient, RestaurantClient>(provider =>
            {
                var client = CreateApiHttpClient(provider, apiBaseUrl);
                return new RestaurantClient(client, apiBaseUrl);
            });

            serviceCollection.AddTransient<IOrderClient, OrderClient>(provider =>
            {
                var client = CreateApiHttpClient(provider, apiBaseUrl);
                return new OrderClient(client, apiBaseUrl);
            });

            serviceCollection.AddTransient<IFoodClient, FoodClient>(provider =>
            {
                var client = CreateApiHttpClient(provider, apiBaseUrl);
                return new FoodClient(client, apiBaseUrl);
            });

            serviceCollection.Scan(selector =>
                selector.FromAssemblyOf<WebBLInstaller>()
                    .AddClasses(classes => classes.AssignableTo<IAppFacade>())
                    .AsSelfWithInterfaces()
                    .WithTransientLifetime());
        }

        public HttpClient CreateApiHttpClient(IServiceProvider serviceProvider, string apiBaseUrl)
        {
            var client = new HttpClient() { BaseAddress = new Uri(apiBaseUrl) };
            client.BaseAddress = new Uri(apiBaseUrl);
            return client;
        }
    }
}
