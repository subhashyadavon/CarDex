using System;
using CarDexBackend.Domain.Entities;
using Xunit;

namespace CarDexBackend.Tests.UnitTests.Domain.Entities
{
    public class PackTests
    {
        [Fact]
        public void Constructor_ShouldInitializeAllPropertiesCorrectly()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var collectionId = Guid.NewGuid();
            int value = 250;

            // Act
            var pack = new Pack(id, userId, collectionId, value);

            // Assert
            Assert.Equal(id, pack.Id);
            Assert.Equal(userId, pack.UserId);
            Assert.Equal(collectionId, pack.CollectionId);
            Assert.Equal(value, pack.Value);
        }

        [Fact]
        public void UpdateValue_ShouldUpdateValue_WhenNewValueIsValid()
        {
            // Arrange
            var pack = new Pack(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 100);

            // Act
            pack.UpdateValue(200);

            // Assert
            Assert.Equal(200, pack.Value);
        }

        [Fact]
        public void UpdateValue_ShouldThrowException_WhenNewValueIsNegative()
        {
            // Arrange
            var pack = new Pack(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 100);

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => pack.UpdateValue(-10));
            Assert.Equal("Value cannot be negative", exception.Message);
        }
    }
}
