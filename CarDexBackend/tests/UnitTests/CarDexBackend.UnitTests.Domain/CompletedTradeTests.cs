using System;
using CarDexBackend.Domain.Entities;
using CarDexBackend.Domain.Enums;
using Xunit;

namespace CarDexBackend.Tests.UnitTests.Domain.Entities
{
    public class CompletedTradeTests
    {
        [Fact]
        public void Constructor_ShouldInitializeProperties_ForPriceTrade()
        {
            // Arrange
            var id = Guid.NewGuid();
            var sellerUserId = Guid.NewGuid();
            var sellerCardId = Guid.NewGuid();
            var buyerUserId = Guid.NewGuid();
            var price = 1000;

            // Act
            var trade = new CompletedTrade(id, TradeEnum.FOR_PRICE, sellerUserId, sellerCardId, buyerUserId, price);

            // Assert
            Assert.Equal(id, trade.Id);
            Assert.Equal(TradeEnum.FOR_PRICE, trade.Type);
            Assert.Equal(sellerUserId, trade.SellerUserId);
            Assert.Equal(sellerCardId, trade.SellerCardId);
            Assert.Equal(buyerUserId, trade.BuyerUserId);
            Assert.Equal(price, trade.Price);
            Assert.Null(trade.BuyerCardId);
            Assert.True((DateTime.UtcNow - trade.ExecutedDate).TotalSeconds < 3);
        }

        [Fact]
        public void Constructor_ShouldInitializeProperties_ForCardTrade()
        {
            // Arrange
            var id = Guid.NewGuid();
            var sellerUserId = Guid.NewGuid();
            var sellerCardId = Guid.NewGuid();
            var buyerUserId = Guid.NewGuid();
            var buyerCardId = Guid.NewGuid();

            // Act
            var trade = new CompletedTrade(id, TradeEnum.FOR_CARD, sellerUserId, sellerCardId, buyerUserId, buyerCardId: buyerCardId);

            // Assert
            Assert.Equal(TradeEnum.FOR_CARD, trade.Type);
            Assert.Equal(buyerCardId, trade.BuyerCardId);
            Assert.Equal(0, trade.Price);
            Assert.True((DateTime.UtcNow - trade.ExecutedDate).TotalSeconds < 3);
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenForCardTradeWithoutBuyerCardId()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() =>
                new CompletedTrade(id, TradeEnum.FOR_CARD, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()));
            Assert.Equal("BuyerCardId must be provided for FOR_CARD trades.", ex.Message);
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenForPriceTradeWithInvalidPrice()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() =>
                new CompletedTrade(id, TradeEnum.FOR_PRICE, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), price: 0));
            Assert.Equal("Price must be greater than 0 for FOR_PRICE trades.", ex.Message);
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenForPriceTradeWithNegativePrice()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() =>
                new CompletedTrade(id, TradeEnum.FOR_PRICE, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), price: -100));
            Assert.Equal("Price must be greater than 0 for FOR_PRICE trades.", ex.Message);
        }
    }
}
