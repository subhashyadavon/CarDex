using System;
using CarDexBackend.Domain.Entities;
using CarDexBackend.Domain.Enums;
using Xunit;

namespace CarDexBackend.Tests.UnitTests.Domain.Entities
{
    public class OpenTradeTests
    {
        [Fact]
        public void Constructor_ShouldInitializeProperties_ForPriceTrade()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var cardId = Guid.NewGuid();
            var price = 500;

            // Act
            var trade = new OpenTrade(id, TradeEnum.ForPrice, userId, cardId, price);

            // Assert
            Assert.Equal(id, trade.Id);
            Assert.Equal(TradeEnum.ForPrice, trade.Type);
            Assert.Equal(userId, trade.UserId);
            Assert.Equal(cardId, trade.CardId);
            Assert.Equal(price, trade.Price);
            Assert.Null(trade.WantCardId);
            Assert.True((DateTime.UtcNow - trade.CreatedAt).TotalSeconds < 3);
        }

        [Fact]
        public void Constructor_ShouldInitializeProperties_ForCardTrade()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var cardId = Guid.NewGuid();
            var wantCardId = Guid.NewGuid();

            // Act
            var trade = new OpenTrade(id, TradeEnum.ForCard, userId, cardId, wantCardId: wantCardId);

            // Assert
            Assert.Equal(TradeEnum.ForCard, trade.Type);
            Assert.Equal(wantCardId, trade.WantCardId);
            Assert.Equal(0, trade.Price);
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenForCardTradeWithoutWantCardId()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() =>
                new OpenTrade(id, TradeEnum.ForCard, Guid.NewGuid(), Guid.NewGuid()));
            Assert.Equal("WantCardId must be provided for ForCard trades.", ex.Message);
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenForPriceTradeWithInvalidPrice()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() =>
                new OpenTrade(id, TradeEnum.ForPrice, Guid.NewGuid(), Guid.NewGuid(), price: 0));
            Assert.Equal("Price must be greater than 0 for ForPrice trades.", ex.Message);
        }

        [Fact]
        public void UpdatePrice_ShouldChangePrice_WhenValid()
        {
            // Arrange
            var trade = new OpenTrade(Guid.NewGuid(), TradeEnum.ForPrice, Guid.NewGuid(), Guid.NewGuid(), 200);

            // Act
            trade.UpdatePrice(400);

            // Assert
            Assert.Equal(400, trade.Price);
        }

        [Fact]
        public void UpdatePrice_ShouldThrowException_WhenTradeIsForCard()
        {
            // Arrange
            var trade = new OpenTrade(Guid.NewGuid(), TradeEnum.ForCard, Guid.NewGuid(), Guid.NewGuid(), wantCardId: Guid.NewGuid());

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => trade.UpdatePrice(500));
            Assert.Equal("Only ForPrice trades can update price.", ex.Message);
        }

        [Fact]
        public void UpdatePrice_ShouldThrowException_WhenNewPriceInvalid()
        {
            // Arrange
            var trade = new OpenTrade(Guid.NewGuid(), TradeEnum.ForPrice, Guid.NewGuid(), Guid.NewGuid(), 200);

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => trade.UpdatePrice(0));
            Assert.Equal("Price must be greater than 0.", ex.Message);
        }
    }
}

