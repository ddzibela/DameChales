using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DameChales.API.DAL.Common.Entities;
using DameChales.Common.Enums;
using DameChales.Common.Models;
using Newtonsoft.Json.Linq;
using Xunit;

namespace DameChales.API.App.EndToEndTests
{
    public class ControllerTests : IAsyncDisposable
    {
        private readonly DameChalesApiApplicationFactory application;
        private readonly Lazy<HttpClient> client;

        public ControllerTests()
        {
            application = new DameChalesApiApplicationFactory();
            client = new Lazy<HttpClient>(application.CreateClient());
        }

        // FOOD TESTS

        [Fact]
        public async Task PostAndDeleteFood_Test()
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

            response = await client.Value.DeleteAsync("/api/food/96103111-393b-46b8-8b4f-ec82212cffba");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task UpsertAndDeleteFood_Test()
        {

            var myObject = new
            {
                id = "96103111-393b-46b8-8b4f-ec82212cffba",
                name = "string",
                photoURL = "string",
                description = "string",
                price = 0,
                restaurantGuid = "75970373-0afa-4c9b-9bc3-2655f3c1efe0",
                alergens = new HashSet<Alergens>()
            };

            JsonContent content = JsonContent.Create(myObject);

            var response = await client.Value.PostAsync("/api/food/upsert", content);

            response.EnsureSuccessStatusCode();

            response = await client.Value.DeleteAsync("/api/food/96103111-393b-46b8-8b4f-ec82212cffba");

            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateFood_Test()
        {

            var myObject = new
            {
                id = "96103111-393b-46b8-8b4f-ec82212cffbf",
                name = "string",
                photoURL = "string",
                description = "string",
                price = 0,
                restaurantGuid = "75970373-0afa-4c9b-9bc3-2655f3c1efe0",
                alergens = new HashSet<Alergens>()
            };

            JsonContent content = JsonContent.Create(myObject);

            var response = await client.Value.PutAsync("/api/food", content);

            response.EnsureSuccessStatusCode();
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
        }

        [Fact]
        public async Task GetApiFoodNameRegex()
        {
            var response = await client.Value.GetAsync("/api/food/name/Vajicka%20s%20orechy");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetApiFoodNoAlergens()
        {
            var response = await client.Value.GetAsync("/api/food/restaurant/96103111-393b-46b8-8b4f-ec82212cffbf/noalergens/1_2_3");

            response.EnsureSuccessStatusCode();
        }

        // ORDER TESTS

        [Fact]
        public async Task UpdateOrder_Test()
        {
            var httpContent = new StringContent("{\r\n  \"id\": \"e184748d-b151-4129-83f9-f2ac2486fa55\",\r\n  \"restaurantGuid\": \"75970373-0afa-4c9b-9bc3-2655f3c1efe0\",\r\n  \"name\": \"Dominik Petrik\",\r\n  \"note\": \"Poznamka k objednavce.\",\r\n  \"deliveryTime\": \"00:15:00\",\r\n  \"status\": 0,\r\n  \"foodAmounts\": [\r\n    {\r\n      \"id\": \"67ecbe97-ba81-490d-9f9a-11c4832b4e94\",\r\n      \"amount\": 1,\r\n      \"note\": \"poznamka\",\r\n      \"food\": {\r\n        \"id\": \"96103111-393b-46b8-8b4f-ec82212cffbf\",\r\n        \"name\": \"Vajicka s orechy\",\r\n        \"photoURL\": \"https://upload.wikimedia.org/wikipedia/commons/thumb/5/5e/Chicken_egg_2009-06-04.jpg/428px-Chicken_egg_2009-06-04.jpg\",\r\n        \"price\": 150\r\n      }\r\n    },\r\n    {\r\n      \"id\": \"3b9f8a14-b6ed-4701-ab35-b05096c2fccf\",\r\n      \"amount\": 2,\r\n      \"note\": \"\",\r\n      \"food\": {\r\n        \"id\": \"82bff672-382c-49e9-aca2-52dd028414a3\",\r\n        \"name\": \"Cibule na slehacce\",\r\n        \"photoURL\": \"https://upload.wikimedia.org/wikipedia/commons/thumb/2/25/Onion_on_White.JPG/480px-Onion_on_White.JPG\",\r\n        \"price\": 100.5\r\n      }\r\n    }\r\n  ]\r\n}");
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");


            var response = await client.Value.PutAsync("/api/order", httpContent);

            response.EnsureSuccessStatusCode();

        }

        [Fact]
        public async Task PostAndDeleteOrder_Test()
        {
            var httpContent = new StringContent("{\r\n  \"id\": \"e184748d-b151-4129-83f9-f2ac2486fa51\",\r\n  \"restaurantGuid\": \"75970373-0afa-4c9b-9bc3-2655f3c1efe0\",\r\n  \"name\": \"Dominik Petrik\",\r\n  \"note\": \"Poznamka k objednavce.\",\r\n  \"deliveryTime\": \"00:15:00\",\r\n  \"status\": 0,\r\n  \"foodAmounts\": [\r\n    {\r\n      \"id\": \"67ecbe97-ba81-490d-9f9a-11c4832b4e94\",\r\n      \"amount\": 1,\r\n      \"note\": \"poznamka\",\r\n      \"food\": {\r\n        \"id\": \"96103111-393b-46b8-8b4f-ec82212cffbf\",\r\n        \"name\": \"Vajicka s orechy\",\r\n        \"photoURL\": \"https://upload.wikimedia.org/wikipedia/commons/thumb/5/5e/Chicken_egg_2009-06-04.jpg/428px-Chicken_egg_2009-06-04.jpg\",\r\n        \"price\": 150\r\n      }\r\n    },\r\n    {\r\n      \"id\": \"3b9f8a14-b6ed-4701-ab35-b05096c2fccf\",\r\n      \"amount\": 2,\r\n      \"note\": \"\",\r\n      \"food\": {\r\n        \"id\": \"82bff672-382c-49e9-aca2-52dd028414a3\",\r\n        \"name\": \"Cibule na slehacce\",\r\n        \"photoURL\": \"https://upload.wikimedia.org/wikipedia/commons/thumb/2/25/Onion_on_White.JPG/480px-Onion_on_White.JPG\",\r\n        \"price\": 100.5\r\n      }\r\n    }\r\n  ]\r\n}");
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");


            var response = await client.Value.PostAsync("/api/order", httpContent);

            response.EnsureSuccessStatusCode();

            response = await client.Value.DeleteAsync("/api/order/e184748d-b151-4129-83f9-f2ac2486fa51");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task UpsertAndDeleteOrder_Test()
        {
            var httpContent = new StringContent("{\r\n  \"id\": \"e184748d-b151-4129-83f9-f2ac2486fa51\",\r\n  \"restaurantGuid\": \"75970373-0afa-4c9b-9bc3-2655f3c1efe0\",\r\n  \"name\": \"Dominik Petrik\",\r\n  \"note\": \"Poznamka k objednavce.\",\r\n  \"deliveryTime\": \"00:15:00\",\r\n  \"status\": 0,\r\n  \"foodAmounts\": [\r\n    {\r\n      \"id\": \"67ecbe97-ba81-490d-9f9a-11c4832b4e94\",\r\n      \"amount\": 1,\r\n      \"note\": \"poznamka\",\r\n      \"food\": {\r\n        \"id\": \"96103111-393b-46b8-8b4f-ec82212cffbf\",\r\n        \"name\": \"Vajicka s orechy\",\r\n        \"photoURL\": \"https://upload.wikimedia.org/wikipedia/commons/thumb/5/5e/Chicken_egg_2009-06-04.jpg/428px-Chicken_egg_2009-06-04.jpg\",\r\n        \"price\": 150\r\n      }\r\n    },\r\n    {\r\n      \"id\": \"3b9f8a14-b6ed-4701-ab35-b05096c2fccf\",\r\n      \"amount\": 2,\r\n      \"note\": \"\",\r\n      \"food\": {\r\n        \"id\": \"82bff672-382c-49e9-aca2-52dd028414a3\",\r\n        \"name\": \"Cibule na slehacce\",\r\n        \"photoURL\": \"https://upload.wikimedia.org/wikipedia/commons/thumb/2/25/Onion_on_White.JPG/480px-Onion_on_White.JPG\",\r\n        \"price\": 100.5\r\n      }\r\n    }\r\n  ]\r\n}");
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");


            var response = await client.Value.PostAsync("/api/order/upsert", httpContent);

            response.EnsureSuccessStatusCode();

            response = await client.Value.DeleteAsync("/api/order/e184748d-b151-4129-83f9-f2ac2486fa51");

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

        // RESTAURANT TESTS

        [Fact]
        public async Task PostAndDeleteRestaurant_Test()
        {
            var httpContent = new StringContent("{\r\n  \"id\": \"75970373-0afa-4c9b-9bc3-2655f3c1efc1\",\r\n  \"name\": \"SkvelaRestaurace\",\r\n  \"description\": \"Mame nejlepsi vajicka\",\r\n  \"photoURL\": \"https://m.facebook.com/eggotruckbrno/\",\r\n  \"address\": \"Dvořákova 12, Brno, Czech Republic\",\r\n  \"gpsCoordinates\": \"49.195942, 16.611404\",\r\n  \"foods\": [\r\n    {\r\n      \"id\": \"96103111-393b-46b8-8b4f-ec82212cffbf\",\r\n      \"name\": \"Vajicka s orechy\",\r\n      \"photoURL\": \"https://upload.wikimedia.org/wikipedia/commons/thumb/5/5e/Chicken_egg_2009-06-04.jpg/428px-Chicken_egg_2009-06-04.jpg\",\r\n      \"price\": 150\r\n    },\r\n    {\r\n      \"id\": \"82bff672-382c-49e9-aca2-52dd028414a3\",\r\n      \"name\": \"Cibule na slehacce\",\r\n      \"photoURL\": \"https://upload.wikimedia.org/wikipedia/commons/thumb/2/25/Onion_on_White.JPG/480px-Onion_on_White.JPG\",\r\n      \"price\": 100.5\r\n    }\r\n  ],\r\n  \"orders\": [\r\n    {\r\n      \"id\": \"e184748d-b151-4129-83f9-f2ac2486fa55\",\r\n      \"restaurantGuid\": \"75970373-0afa-4c9b-9bc3-2655f3c1efe0\",\r\n      \"name\": \"Dominik Petrik\",\r\n      \"status\": 0\r\n    }\r\n  ]\r\n}");
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");


            var response = await client.Value.PostAsync("/api/restaurant", httpContent);

            response.EnsureSuccessStatusCode();

            response = await client.Value.DeleteAsync("/api/restaurant/75970373-0afa-4c9b-9bc3-2655f3c1efc1");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task UpsertAndDeleteRestaurant_Test()
        {
            var httpContent = new StringContent("{\r\n  \"id\": \"75970373-0afa-4c9b-9bc3-2655f3c1efc1\",\r\n  \"name\": \"SkvelaRestaurace\",\r\n  \"description\": \"Mame nejlepsi vajicka\",\r\n  \"photoURL\": \"https://m.facebook.com/eggotruckbrno/\",\r\n  \"address\": \"Dvořákova 12, Brno, Czech Republic\",\r\n  \"gpsCoordinates\": \"49.195942, 16.611404\",\r\n  \"foods\": [\r\n    {\r\n      \"id\": \"96103111-393b-46b8-8b4f-ec82212cffbf\",\r\n      \"name\": \"Vajicka s orechy\",\r\n      \"photoURL\": \"https://upload.wikimedia.org/wikipedia/commons/thumb/5/5e/Chicken_egg_2009-06-04.jpg/428px-Chicken_egg_2009-06-04.jpg\",\r\n      \"price\": 150\r\n    },\r\n    {\r\n      \"id\": \"82bff672-382c-49e9-aca2-52dd028414a3\",\r\n      \"name\": \"Cibule na slehacce\",\r\n      \"photoURL\": \"https://upload.wikimedia.org/wikipedia/commons/thumb/2/25/Onion_on_White.JPG/480px-Onion_on_White.JPG\",\r\n      \"price\": 100.5\r\n    }\r\n  ],\r\n  \"orders\": [\r\n    {\r\n      \"id\": \"e184748d-b151-4129-83f9-f2ac2486fa55\",\r\n      \"restaurantGuid\": \"75970373-0afa-4c9b-9bc3-2655f3c1efe0\",\r\n      \"name\": \"Dominik Petrik\",\r\n      \"status\": 0\r\n    }\r\n  ]\r\n}");
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");


            var response = await client.Value.PostAsync("/api/restaurant/upsert", httpContent);

            response.EnsureSuccessStatusCode();

            response = await client.Value.DeleteAsync("/api/restaurant/75970373-0afa-4c9b-9bc3-2655f3c1efc1");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task UpdateRestaurant_Test()
        {
            var httpContent = new StringContent("{\r\n  \"id\": \"75970373-0afa-4c9b-9bc3-2655f3c1efe0\",\r\n  \"name\": \"NameUpdated\",\r\n  \"description\": \"Mame nejlepsi vajicka\",\r\n  \"photoURL\": \"https://m.facebook.com/eggotruckbrno/\",\r\n  \"address\": \"Dvořákova 12, Brno, Czech Republic\",\r\n  \"gpsCoordinates\": \"49.195942, 16.611404\",\r\n  \"foods\": [\r\n    {\r\n      \"id\": \"96103111-393b-46b8-8b4f-ec82212cffbf\",\r\n      \"name\": \"Vajicka s orechy\",\r\n      \"photoURL\": \"https://upload.wikimedia.org/wikipedia/commons/thumb/5/5e/Chicken_egg_2009-06-04.jpg/428px-Chicken_egg_2009-06-04.jpg\",\r\n      \"price\": 150\r\n    },\r\n    {\r\n      \"id\": \"82bff672-382c-49e9-aca2-52dd028414a3\",\r\n      \"name\": \"Cibule na slehacce\",\r\n      \"photoURL\": \"https://upload.wikimedia.org/wikipedia/commons/thumb/2/25/Onion_on_White.JPG/480px-Onion_on_White.JPG\",\r\n      \"price\": 100.5\r\n    }\r\n  ],\r\n  \"orders\": [\r\n    {\r\n      \"id\": \"e184748d-b151-4129-83f9-f2ac2486fa55\",\r\n      \"restaurantGuid\": \"75970373-0afa-4c9b-9bc3-2655f3c1efe0\",\r\n      \"name\": \"Dominik Petrik\",\r\n      \"status\": 0\r\n    }\r\n  ]\r\n}");
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");


            var response = await client.Value.PutAsync("/api/restaurant", httpContent);

            response.EnsureSuccessStatusCode();

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
        }

        [Fact]
        public async Task GetByAdressIdRegex()
        {
            var response = await client.Value.GetAsync("/api/restaurant/address/Dvo%C5%99%C3%A1kova%2012%2C%20Brno%2C%20Czech%20Republic");

            response.EnsureSuccessStatusCode();

        }

        [Fact]
        public async Task GetRestaurantByFoodId()
        {
            var response = await client.Value.GetAsync("/api/restaurant/food/96103111-393b-46b8-8b4f-ec82212cffbf");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetRestaurantEarnings()
        {
            var response = await client.Value.GetAsync("/api/restaurant/earnings/75970373-0afa-4c9b-9bc3-2655f3c1efe0");

            response.EnsureSuccessStatusCode();
        }

        public async ValueTask DisposeAsync()
        {
            await application.DisposeAsync();
        }
    }
}
