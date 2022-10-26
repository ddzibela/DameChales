using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DameChales.Common.Models;
using Xunit;

namespace DameChales.API.App.EndToEndTests
{
    public class OrderControllerTests : IAsyncDisposable
    {
        private readonly DameChalesApiApplicationFactory application;
        private readonly Lazy<HttpClient> client;

        public OrderControllerTests()
        {
            application = new DameChalesApiApplicationFactory();
            client = new Lazy<HttpClient>(application.CreateClient());
        }

        [Fact]
        public async Task GetAllOrders_Returns_At_Last_One_Recipe()
        {
            var response = await client.Value.GetAsync("/api/order");

            response.EnsureSuccessStatusCode();

            var recipes = await response.Content.ReadFromJsonAsync<ICollection<OrderListModel>>();
            Assert.NotNull(recipes);
            Assert.NotEmpty(recipes);
        }

        public async ValueTask DisposeAsync()
        {
            await application.DisposeAsync();
        }
    }
}
