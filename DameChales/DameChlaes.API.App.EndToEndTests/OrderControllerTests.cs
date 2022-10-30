using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DameChales.Common.Enums;
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
        public async Task PostFood_Test()
        {

            var myObject = new
            {
                id= "96103111-393b-46b8-8b4f-ec82212cffba",
                name="string",
                photoURL="string",
                description="string",
                price=0,
                restaurantGuid= "75970373-0afa-4c9b-9bc3-2655f3c1efe0",
                alergens = new HashSet<Alergens>()
            };

            JsonContent content = JsonContent.Create(myObject);

            var response = await client.Value.PostAsync("/api/food", content);

            response.EnsureSuccessStatusCode();
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

        [Fact]
        public async Task GetByIdFood()
        {
            var response = await client.Value.GetAsync("/api/food/96103111-393b-46b8-8b4f-ec82212cffbf");

            response.EnsureSuccessStatusCode();

            var foods = await response.Content.ReadFromJsonAsync<FoodDetailModel>();
            Assert.NotNull(foods);
        }

        [Fact]
        public async Task GetApiFoodRestaurantId()
        {
            var response = await client.Value.GetAsync("/api/food/restaurant/96103111-393b-46b8-8b4f-ec82212cffbf");

            response.EnsureSuccessStatusCode();

            Assert.NotNull(response.StatusCode);
        }

        [Fact]
        public async Task GetApiFoodNameRegex()
        {
            var response = await client.Value.GetAsync("/api/food/name/Vajicka%20s%20orechy");

            response.EnsureSuccessStatusCode();

            Assert.NotNull(response.StatusCode);
        }

        [Fact]
        public async Task GetApiFoodNoAlergens()
        {
            var response = await client.Value.GetAsync("/api/food/restaurant/96103111-393b-46b8-8b4f-ec82212cffbf/noalergens/1_2_3");

            response.EnsureSuccessStatusCode();

            Assert.NotNull(response.StatusCode);
        }

        //orders tests

        [Fact]
        public async Task GetByIdOrder()
        {
            var response = await client.Value.GetAsync("/api/order/e184748d-b151-4129-83f9-f2ac2486fa55");

            response.EnsureSuccessStatusCode();

            var orders = await response.Content.ReadFromJsonAsync<OrderListModel>();
            Assert.NotNull(orders);
        }

        [Fact]
        public async Task GetByIdOrderFood()
        {
            var response = await client.Value.GetAsync("/api/order/food/e184748d-b151-4129-83f9-f2ac2486fa55");

            response.EnsureSuccessStatusCode();

            Assert.NotNull(response.StatusCode);
        }

        [Fact]
        public async Task GetByIdOrderRestaurant()
        {
            var response = await client.Value.GetAsync("/api/order/restaurant/96103111-393b-46b8-8b4f-ec82212cffbf");

            response.EnsureSuccessStatusCode();

            Assert.NotNull(response.StatusCode);
        }

        [Fact]
        public async Task GetByIdOrderRestaurantStatus()
        {
            var response = await client.Value.GetAsync("/api/order/restaurant/96103111-393b-46b8-8b4f-ec82212cffbf/status/1");

            response.EnsureSuccessStatusCode();

            Assert.NotNull(response.StatusCode);
        }

        [Fact]
        public async Task GetByRestaurantId()
        {
            var response = await client.Value.GetAsync("/api/restaurant/75970373-0afa-4c9b-9bc3-2655f3c1efe0");

            response.EnsureSuccessStatusCode();

            var restaurants = await response.Content.ReadFromJsonAsync<RestaurantListModel>();
            Assert.NotNull(restaurants);
        }

        [Fact]
        public async Task GetByRestaurantIdRegex()
        {
            var response = await client.Value.GetAsync("/api/restaurant/name/SkvelaRestaurace");

            response.EnsureSuccessStatusCode();

            Assert.NotNull(response.StatusCode);
        }

        [Fact]
        public async Task GetByAdressIdRegex()
        {
            var response = await client.Value.GetAsync("/api/restaurant/address/Dvo%C5%99%C3%A1kova%2012%2C%20Brno%2C%20Czech%20Republic");

            response.EnsureSuccessStatusCode();

            Assert.NotNull(response.StatusCode);
        }

        [Fact]
        public async Task GetByIdRestaurantEarnings()
        {
            var response = await client.Value.GetAsync("/api/restaurant/food/96103111-393b-46b8-8b4f-ec82212cffbf");

            response.EnsureSuccessStatusCode();

            Assert.NotNull(response.StatusCode);
        }

        public async ValueTask DisposeAsync()
        {
            await application.DisposeAsync();
        }
    }
}
