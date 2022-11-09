using System.Net.Http;

namespace DameChales.Web.BL;

public partial class RestaurantApiClient
{
    public RestaurantApiClient(HttpClient httpClient, string baseUrl)
        : this(httpClient)
    {
        BaseUrl = baseUrl;
    }
}
