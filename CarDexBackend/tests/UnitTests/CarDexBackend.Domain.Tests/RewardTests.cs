using System;
using CarDexBackend.Domain.Entities;
using CarDexBackend.Domain.Enums;
using Xunit;

namespace CarDexBackend.Tests.UnitTests.Domain.Entities
{
    public class RewardTests
    {
        [Fact]
        public void Constructor_ShouldInitializeAllPropertiesCorrectly()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var itemId = Guid.NewGuid();
            var type = RewardEnum.CURRENCY;
            int amount = 100;

            // Act
            var reward = new Reward(id, userId, type, amount, itemId);

            // Assert
            Assert.Equal(id, reward.Id);
            Assert.Equal(userId, reward.UserId);
            Assert.Equal(type, reward.Type);
            Assert.Equal(itemId, reward.ItemId);
            Assert.Equal(amount, reward.Amount);
            Assert.Null(reward.ClaimedAt);
        }

        [Fact]
        public void Constructor_ShouldAllowNullItemId()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();

            // Act
            var reward = new Reward(id, userId, RewardEnum.CURRENCY, 50);

            // Assert
            Assert.Null(reward.ItemId);
        }

        [Fact]
        public void Claim_ShouldSetClaimedAt_WhenCalledFirstTime()
        {
            // Arrange
            var reward = new Reward(Guid.NewGuid(), Guid.NewGuid(), RewardEnum.CURRENCY, 100);

            // Act
            reward.Claim();

            // Assert
            Assert.NotNull(reward.ClaimedAt);
            Assert.True(reward.IsClaimed());
        }

        [Fact]
        public void Claim_ShouldThrowException_WhenAlreadyClaimed()
        {
            // Arrange
            var reward = new Reward(Guid.NewGuid(), Guid.NewGuid(), RewardEnum.CURRENCY, 100);
            reward.Claim();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => reward.Claim());
        }

        [Fact]
        public void IsClaimed_ShouldReturnFalse_WhenNotClaimed()
        {
            // Arrange
            var reward = new Reward(Guid.NewGuid(), Guid.NewGuid(), RewardEnum.CURRENCY, 100);

            // Act
            var result = reward.IsClaimed();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsClaimed_ShouldReturnTrue_WhenClaimed()
        {
            // Arrange
            var reward = new Reward(Guid.NewGuid(), Guid.NewGuid(), RewardEnum.CURRENCY, 100);
            reward.Claim();

            // Act
            var result = reward.IsClaimed();

            // Assert
            Assert.True(result);
        }
    }
}
