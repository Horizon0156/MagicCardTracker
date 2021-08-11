
namespace MagicCardTracker.ScryfallClient
{
    /// <inheritdoc />
    public partial class CardsClient
    {
        /// <inheritdoc />
        partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, System.Text.StringBuilder urlBuilder)
        {
            //  We need to restore escaped '+' characters as this is a special command
            // character in the Scryfall Search Syntax
            urlBuilder.Replace("%2B", "+");
        }
    }
}