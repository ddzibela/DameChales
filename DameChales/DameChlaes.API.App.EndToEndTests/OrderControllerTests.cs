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
        public async Task GetAllOrders_Returns_At_Last_One_Order()
        {
            var response = await client.Value.GetAsync("/api/order");

            response.EnsureSuccessStatusCode();

            var orders = await response.Content.ReadFromJsonAsync<ICollection<OrderListModel>>();
            Assert.NotNull(orders);
            Assert.NotEmpty(orders);
        }

        [Fact]
        public async Task GetAllRestaurants_Returns_At_Last_One_Restaurant()
        {
            var response = await client.Value.GetAsync("/api/restaurant");

            response.EnsureSuccessStatusCode();

            var restaurants = await response.Content.ReadFromJsonAsync<ICollection<RestaurantListModel>>();
            Assert.NotNull(restaurants);
            Assert.NotEmpty(restaurants);
        }

        [Fact]
        public async Task GetAllFoods_Returns_At_Last_One_Food()
        {
            var response = await client.Value.GetAsync("/api/food");

            response.EnsureSuccessStatusCode();

            var foods = await response.Content.ReadFromJsonAsync<ICollection<FoodListModel>>();
            Assert.NotNull(foods);
            Assert.NotEmpty(foods);
        }

        public async ValueTask DisposeAsync()
        {
            await application.DisposeAsync();
        }
    }
}
