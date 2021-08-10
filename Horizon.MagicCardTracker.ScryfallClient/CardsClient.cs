
using System.Net.Http;
using System.Text;

namespace Horizon.MagicCardTracker.ScryfallClient
{
    public partial class CardsClient
    {
        partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, System.Text.StringBuilder urlBuilder)
        {
            urlBuilder.Replace("%2B", "+");
        }
    }
}