using NSubstitute;
using Xunit;
using MagicCardTracker.Contracts;
using MagicCardTracker.Storage.Abstrations;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

namespace MagicCardTracker.Storage.Tests
{
    public class SingleUserCardLibraryTests
    {
        [Fact]
        public async Task TestAddCardShouldRecoverLibraryIfCalledForTheFirstTime()
        {
            var persister = Substitute.For<ICardLibraryPersister>();
            var sut = new SingleUserCardLibrary(persister);

            await sut.AddCardAsync(new CollectedCard(), CancellationToken.None);
            await persister.Received().RestoreLibraryAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task TestAddCardShouldNotRecoverLibraryIfCalledForTheSecondTime()
        {
            var persister = Substitute.For<ICardLibraryPersister>();
            var sut = new SingleUserCardLibrary(persister);

            await sut.AddCardAsync(new CollectedCard(), CancellationToken.None);
            await sut.AddCardAsync(new CollectedCard(), CancellationToken.None);
            await persister.Received(1).RestoreLibraryAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task TestAddCardShouldNotAddCardWithCountZero()
        {
            var persister = Substitute.For<ICardLibraryPersister>();
            var sut = new SingleUserCardLibrary(persister);

            var card = new CollectedCard
            {
                SetCode = "soi",
                Number = "97",
                LanguageCode = "en",
                Count = 0
            };
            await sut.AddCardAsync(card, CancellationToken.None);
            var cards = await sut.GetCollectedCardsAsync(CancellationToken.None);
            Assert.Empty(cards);
        }

        [Fact]
        public async Task TestAddCardShouldAddCardWithCountGreaterZero()
        {
            var persister = Substitute.For<ICardLibraryPersister>();
            var sut = new SingleUserCardLibrary(persister);

            var card = new CollectedCard
            {
                SetCode = "soi",
                Number = "97",
                LanguageCode = "en",
                Count = 1
            };
            await sut.AddCardAsync(card, CancellationToken.None);
            var cards = await sut.GetCollectedCardsAsync(CancellationToken.None);
            Assert.Single(cards);
        }

        [Fact]
        public async Task TestAddSameCardShouldNotAddDuplicate()
        {
            var persister = Substitute.For<ICardLibraryPersister>();
            var sut = new SingleUserCardLibrary(persister);

            var card1 = new CollectedCard
            {
                SetCode = "soi",
                Number = "97",
                LanguageCode = "en",
                Count = 1
            };

            var card2 = new CollectedCard
            {
                SetCode = "soi",
                Number = "97",
                LanguageCode = "en",
                Count = 2
            };
            await sut.AddCardAsync(card1, CancellationToken.None);
            await sut.AddCardAsync(card2, CancellationToken.None);
            var cards = await sut.GetCollectedCardsAsync(CancellationToken.None);
            Assert.Single(cards);
            Assert.Equal(2, cards.FirstOrDefault().Count);
        }

        [Fact]
        public async Task TestAddCardShouldPersistCollection()
        {
            var persister = Substitute.For<ICardLibraryPersister>();
            var sut = new SingleUserCardLibrary(persister);

            var card = new CollectedCard
            {
                SetCode = "soi",
                Number = "97",
                LanguageCode = "en",
                Count = 1
            };

            await sut.AddCardAsync(card, CancellationToken.None);
            var collection = await sut.GetCollectedCardsAsync(CancellationToken.None);
            await persister.Received()
                           .PersistLibraryAsync(collection, Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task TestGetCollectionShouldReturnCollectedCards()
        {
            var persister = Substitute.For<ICardLibraryPersister>();
            var sut = new SingleUserCardLibrary(persister);

            var card = new CollectedCard
            {
                SetCode = "soi",
                Number = "97",
                LanguageCode = "en",
                Count = 1
            };

            await sut.AddCardAsync(card, CancellationToken.None);
            var collection = await sut.GetCollectedCardsAsync(CancellationToken.None);
            Assert.Contains(card, collection);
        }

        [Fact]
        public async Task TestGetCollectionShouldRecoverCollectionIfCalledForTheFirstTime()
        {
            var persister = Substitute.For<ICardLibraryPersister>();
            var sut = new SingleUserCardLibrary(persister);

            var collection = await sut.GetCollectedCardsAsync(CancellationToken.None);
            await persister.Received().RestoreLibraryAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task TestGetCollectionNotShouldRecoverCollectionIfCalledForTheSecondTime()
        {
            var persister = Substitute.For<ICardLibraryPersister>();
            var sut = new SingleUserCardLibrary(persister);

            var collection = await sut.GetCollectedCardsAsync(CancellationToken.None);
            var collection2 = await sut.GetCollectedCardsAsync(CancellationToken.None);
            await persister.Received(1).RestoreLibraryAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task TestGetCollectionNotPersistCollection()
        {
            var persister = Substitute.For<ICardLibraryPersister>();
            var sut = new SingleUserCardLibrary(persister);

            var collection = await sut.GetCollectedCardsAsync(CancellationToken.None);
            await persister.DidNotReceive()
                           .PersistLibraryAsync(
                               Arg.Any<IEnumerable<CollectedCard>>(),
                               Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task TestSearchInCollectionShouldRecoverCollectionIfCalledForTheFirstTime()
        {
            var persister = Substitute.For<ICardLibraryPersister>();
            var sut = new SingleUserCardLibrary(persister);

            var card = new CollectedCard
            {
                SetCode = "soi",
                Number = "97",
                LanguageCode = "en",
                Count = 1
            };

            await sut.SearchInCollectionAsync(card, CancellationToken.None);
            await persister.Received().RestoreLibraryAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task TestSearchInCollectionShouldNotRecoverCollectionIfCalledForTheSecondTime()
        {
            var persister = Substitute.For<ICardLibraryPersister>();
            var sut = new SingleUserCardLibrary(persister);

            var card = new CollectedCard
            {
                SetCode = "soi",
                Number = "97",
                LanguageCode = "en",
                Count = 1
            };

            await sut.SearchInCollectionAsync(card, CancellationToken.None);
            await sut.SearchInCollectionAsync(card, CancellationToken.None);
            await persister.Received(1).RestoreLibraryAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task TestSearchInCollectionShouldNotPersistCollection()
        {
            var persister = Substitute.For<ICardLibraryPersister>();
            var sut = new SingleUserCardLibrary(persister);

            var card = new CollectedCard
            {
                SetCode = "soi",
                Number = "97",
                LanguageCode = "en",
                Count = 1
            };

            await sut.SearchInCollectionAsync(card, CancellationToken.None);
            await persister.DidNotReceive()
                           .PersistLibraryAsync(
                               Arg.Any<IEnumerable<CollectedCard>>(),
                               Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task TestSearchInCollectionShouldReturnCountZeroIfCardIsNotCollected()
        {
            var persister = Substitute.For<ICardLibraryPersister>();
            var sut = new SingleUserCardLibrary(persister);

            var card = new Card
            {
                SetCode = "soi",
                Number = "97",
                LanguageCode = "en"
            };

            var result = await sut.SearchInCollectionAsync(card, CancellationToken.None);
            Assert.Equal(0, result.Count);
        }

        [Fact]
        public async Task TestSearchInCollectionShouldReturnActualCountIfCardIsCollected()
        {
            var persister = Substitute.For<ICardLibraryPersister>();
            var sut = new SingleUserCardLibrary(persister);

            var collectedCard = new CollectedCard
            {
                SetCode = "soi",
                Number = "97",
                LanguageCode = "en",
                Count = 2
            };
            await sut.AddCardAsync(collectedCard, CancellationToken.None);

            var card = new Card
            {
                SetCode = "soi",
                Number = "97",
                LanguageCode = "en"
            };

            var result = await sut.SearchInCollectionAsync(card, CancellationToken.None);
            Assert.Equal(collectedCard.Count, result.Count);
        }

        [Fact]
        public async Task TestSearchInCollectionByIdShouldRecoverCollectionIfCalledForTheFirstTime()
        {
            var persister = Substitute.For<ICardLibraryPersister>();
            var sut = new SingleUserCardLibrary(persister);

            await sut.SearchInCollectionByIdAsync("soi", "97", "en", CancellationToken.None);
            await persister.Received().RestoreLibraryAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task TestSearchInCollectionByIdShouldNotRecoverCollectionIfCalledForTheSecondTime()
        {
            var persister = Substitute.For<ICardLibraryPersister>();
            var sut = new SingleUserCardLibrary(persister);

            await sut.SearchInCollectionByIdAsync("soi", "97", "en", CancellationToken.None);
            await sut.SearchInCollectionByIdAsync("soi", "97", "en", CancellationToken.None);
            await persister.Received(1).RestoreLibraryAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task TestSearchInCollectionByIdShouldNotPersistCollection()
        {
            var persister = Substitute.For<ICardLibraryPersister>();
            var sut = new SingleUserCardLibrary(persister);

            var card = new CollectedCard
            {
                SetCode = "soi",
                Number = "97",
                LanguageCode = "en",
                Count = 1
            };

            await sut.SearchInCollectionByIdAsync("soi", "97", "en", CancellationToken.None);
            await persister.DidNotReceive()
                           .PersistLibraryAsync(
                               Arg.Any<IEnumerable<CollectedCard>>(),
                               Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task TestSearchInCollectionByIdShouldReturnNullIfCardIsNotCollected()
        {
            var persister = Substitute.For<ICardLibraryPersister>();
            var sut = new SingleUserCardLibrary(persister);

            var result = await sut.SearchInCollectionByIdAsync("soi", "97", "en", CancellationToken.None);
            Assert.Null(result);
        }

        [Fact]
        public async Task TestSearchInCollectionByIdShouldReturnActualCountIfCardIsCollected()
        {
            var persister = Substitute.For<ICardLibraryPersister>();
            var sut = new SingleUserCardLibrary(persister);

            var collectedCard = new CollectedCard
            {
                SetCode = "soi",
                Number = "97",
                LanguageCode = "en",
                Count = 2
            };
            await sut.AddCardAsync(collectedCard, CancellationToken.None);

            var result = await sut.SearchInCollectionByIdAsync("soi", "97", "en", CancellationToken.None);
            Assert.Equal(collectedCard.Count, result.Count);
        }

        [Fact]
        public async Task TestSetCollectionAsyncShouldTakeTheGivenLibrary()
        {
            var persister = Substitute.For<ICardLibraryPersister>();
            var sut = new SingleUserCardLibrary(persister);

            var oldCard = new CollectedCard
            {
                SetCode = "soi",
                Number = "98",
                LanguageCode = "en",
                Count = 1
            };

            await sut.AddCardAsync(oldCard, CancellationToken.None);

            var collectedCard = new CollectedCard
            {
                SetCode = "soi",
                Number = "97",
                LanguageCode = "en",
                Count = 2
            };
            var collection = new [] { collectedCard };
            await sut.SetCollectionAsync(collection, CancellationToken.None);
            var newCollection = await sut.GetCollectedCardsAsync(CancellationToken.None);
            Assert.Contains(collectedCard, newCollection);
            Assert.Single(newCollection);
        }

        [Fact]
        public async Task TestClearLibraryShouldClearLibrary()
        {
            var persister = Substitute.For<ICardLibraryPersister>();
            var sut = new SingleUserCardLibrary(persister);

            var collectedCard = new CollectedCard
            {
                SetCode = "soi",
                Number = "97",
                LanguageCode = "en",
                Count = 2
            };
            
            await sut.AddCardAsync(collectedCard, CancellationToken.None);
            await sut.ClearCollectionAsync(CancellationToken.None);
            
            var collection = await sut.GetCollectedCardsAsync(CancellationToken.None);
            Assert.Empty(collection);
        }
    }
}
