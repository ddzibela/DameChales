using System.Net.Http;

namespace DameChales.Web.BL;

public partial class RestaurantClient
{
    public RestaurantClient(HttpClient httpClient, string baseUrl)
        : this(httpClient)
    {
        BaseUrl = baseUrl;
    }
}
