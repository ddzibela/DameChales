using System.Net.Http;

namespace DameChales.Web.BL;

public partial class FoodApiClient
{
    public FoodApiClient(HttpClient httpClient, string baseUrl)
        : this(httpClient)
    {
        BaseUrl = baseUrl;
    }
}
