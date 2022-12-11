using System.Net.Http;

namespace DameChales.Web.BL;

public partial class FoodClient
{
    public FoodClient(HttpClient httpClient, string baseUrl)
        : this(httpClient)
    {
        BaseUrl = baseUrl;
    }
}
