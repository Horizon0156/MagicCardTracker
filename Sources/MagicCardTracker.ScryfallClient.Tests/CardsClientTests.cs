using System.Net.Http;
using System.Reflection;
using System.Text;
using NSubstitute;
using Xunit;

namespace MagicCardTracker.ScryfallClient.Tests
{
    public class CardsClientTests
    {
        [Fact]
        public void TestPrepareRequestShouldReplaceEncodedPlusCharacters()
        {
            var httpClient = Substitute.For<HttpClient>();
                                     
            var sut = new CardsClient(httpClient);
            var urlBuilder = new StringBuilder();
            urlBuilder.Append("set%3Aafr%2Blang%3Ade");

            // Extract private partial as this, unfortunately, forced by generated code.
            var methodInfo = typeof(CardsClient).GetMethod(
                "PrepareRequest", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = { null, null, urlBuilder };
            methodInfo.Invoke(sut, parameters);

            Assert.Equal(urlBuilder.ToString(), "set%3Aafr+lang%3Ade");
        }
    }
}
