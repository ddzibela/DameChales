using System.Net.Http;

namespace DameChales.Web.BL;

public partial class OrderApiClient
{
    public OrderApiClient(HttpClient httpClient, string baseUrl)
        : this(httpClient)
    {
        BaseUrl = baseUrl;
    }
}
