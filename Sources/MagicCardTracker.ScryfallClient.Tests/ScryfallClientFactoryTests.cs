using System.Net.Http;
using NSubstitute;
using Xunit;

namespace MagicCardTracker.ScryfallClient.Tests;

public class ScryfallClientFactoryTests
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ScryfallClientFactoryTests()
    {
        _httpClientFactory = Substitute.For<IHttpClientFactory>();
    }

    [Fact]
    public void TestCardsShouldReturnValidClient()
    {
        var sut = new ScryfallClientFactory(_httpClientFactory);
        var cardsClient = sut.Cards;

        Assert.IsType<CardsClient>(cardsClient);
        Assert.NotNull(cardsClient);
    }

    [Fact]
    public void TestSetsShouldReturnValidClient()
    {
        var sut = new ScryfallClientFactory(_httpClientFactory);
        var setsClient = sut.Sets;

        Assert.IsType<SetsClient>(setsClient);
        Assert.NotNull(setsClient);
    }

    [Fact]
    public void TestCatalogShouldReturnValidClient()
    {
        var sut = new ScryfallClientFactory(_httpClientFactory);
        var catalogClient = sut.Catalog;

        Assert.IsType<CatalogClient>(catalogClient);
        Assert.NotNull(catalogClient);
    }

    [Fact]
    public void TestSymbologyShouldReturnValidClient()
    {
        var sut = new ScryfallClientFactory(_httpClientFactory);
        var symbologyClient = sut.Symbology;

        Assert.IsType<SymbologyClient>(symbologyClient);
        Assert.NotNull(symbologyClient);
    }

    [Fact]
    public void TestRulingsShouldReturnValidClient()
    {
        var sut = new ScryfallClientFactory(_httpClientFactory);
        var rulingsClient = sut.Rulings;

        Assert.IsType<RulingsClient>(rulingsClient);
        Assert.NotNull(rulingsClient);
    }
}
