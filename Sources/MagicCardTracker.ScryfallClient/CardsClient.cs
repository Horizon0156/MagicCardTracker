
using System.Net.Http;
using System.Text;

namespace MagicCardTracker.ScryfallClient
{
    /// <inheritdoc />
    public partial class CardsClient
    {
        /// <inheritdoc />
        partial void PrepareRequest(
            HttpClient client,
            HttpRequestMessage request,
            StringBuilder urlBuilder)
        {
            //  We need to restore escaped '+' characters as this is a special command
            // character in the Scryfall Search Syntax
            urlBuilder.Replace("%2B", "+");
        }
    }
}