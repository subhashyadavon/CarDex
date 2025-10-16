
using System;
using Xunit;
using CarDexBackend.Domain.Entities;
using CarDexBackend.Domain.Enums;

namespace CarDexBackend.Domain.Tests
{
    public class UserTests
    {
        [Fact]
        public void AddCurrency_ShouldIncreaseBalance_WhenAmountIsPositive()
        {
            var user = new User(Guid.NewGuid(), "TestUser", "pass");
            user.AddCurrency(100);

            Assert.Equal(100, user.Currency);
        }

          [Fact]
        public void AddCurrency_ShouldThrow_WhenAmountIsNegative()
        {
            var user = new User(Guid.NewGuid(), "TestUser", "pass");
            Assert.Throws<InvalidOperationException>(() => user.AddCurrency(-10));
        }

        [Fact]
        public void DeductCurrency_ShouldReduceBalance_WhenEnoughCurrency()
        {
            var user = new User(Guid.NewGuid(), "TestUser", "pass");
            user.AddCurrency(100);
            user.DeductCurrency(40);

            Assert.Equal(60, user.Currency);
        }

        [Fact]
        public void DeductCurrency_ShouldThrow_WhenInsufficientFunds()
        {
            var user = new User(Guid.NewGuid(), "TestUser", "pass");
            Assert.Throws<InvalidOperationException>(() => user.DeductCurrency(50));
        }

        [Fact]
        public void AddCard_ShouldAddCardToOwnedCards()
        {
            var user = new User(Guid.NewGuid(), "TestUser", "pass");
            // use an existing GradeEnum value (Factory, LimitedRun, or NISMO)
            var card = new Card(Guid.NewGuid(), user.Id, Guid.NewGuid(), Guid.NewGuid(), GradeEnum.FACTORY, 100);

            user.AddCard(card);
            Assert.Contains(card, user.OwnedCards);
        }

        [Fact]
        public void HasCard_ShouldReturnTrue_WhenCardExists()
        {
            var user = new User(Guid.NewGuid(), "TestUser", "pass");
            var card = new Card(Guid.NewGuid(), user.Id, Guid.NewGuid(), Guid.NewGuid(), GradeEnum.FACTORY, 100);
            user.AddCard(card);

            Assert.True(user.HasCard(card.Id));
        }


        [Fact]
        public void AddPack_ShouldAddPackToOwnedPacks()
        {
            var user = new User(Guid.NewGuid(), "TestUser", "pass");
            var pack = new Pack(Guid.NewGuid(), user.Id, Guid.NewGuid(), 50);

            user.AddPack(pack);
            Assert.Contains(pack, user.OwnedPacks);
        }

        [Fact]
        public void AddOpenTrade_ShouldAddTradeToOpenTrades()
        {
            var user = new User(Guid.NewGuid(), "TestUser", "pass");
            var trade = new OpenTrade(Guid.NewGuid(), TradeEnum.FOR_PRICE, user.Id, Guid.NewGuid(), 100);

            user.AddOpenTrade(trade);
            Assert.Contains(trade, user.OpenTrades);
        }

        [Fact]
        public void CompleteTrade_ShouldMoveTradeFromOpenToHistory()
        {
            var user = new User(Guid.NewGuid(), "TestUser", "pass");
            var tradeId = Guid.NewGuid();

            var openTrade = new OpenTrade(tradeId, TradeEnum.FOR_PRICE, user.Id, Guid.NewGuid(), 100);
            user.AddOpenTrade(openTrade);

            
            var completedTrade = new CompletedTrade(
                tradeId,
                TradeEnum.FOR_PRICE,
                user.Id,          // sellerUserId
                Guid.NewGuid(),   // sellerCardId
                Guid.NewGuid(),   // buyerUserId
                100,              // price
                null              // buyerCardId
            );

            user.CompleteTrade(completedTrade);

            Assert.Contains(completedTrade, user.TradeHistory);
            Assert.DoesNotContain(openTrade, user.OpenTrades);
        }

    }
}




