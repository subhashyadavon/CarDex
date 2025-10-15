using System;
using Xunit;
using CarDexBackend.Domain.Entities;
using CarDexBackend.Domain.Enums;

namespace CarDexBackend.Domain.Tests
{
    public class CardTests
    {
        [Fact]
        public void UpdateValue_ShouldChangeValue_WhenNewValueIsValid()
        {
            // Arrange
            var card = new Card(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), GradeEnum.Factory, 100);

            // Act
            card.UpdateValue(200);

            // Assert
            Assert.Equal(200, card.Value);
        }

        [Fact]
        public void UpdateValue_ShouldThrow_WhenNewValueIsNegative()
        {
            // Arrange
            var card = new Card(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), GradeEnum.Factory, 100);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => card.UpdateValue(-10));
        }

        [Fact]
        public void UpgradeGrade_ShouldIncreaseGrade_WhenNewGradeIsHigher()
        {
            // Arrange
            var card = new Card(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), GradeEnum.Factory, 100);

            // Act
            card.UpgradeGrade(GradeEnum.LimitedRun);

            // Assert
            Assert.Equal(GradeEnum.LimitedRun, card.Grade);
        }

        [Fact]
        public void UpgradeGrade_ShouldThrow_WhenNewGradeIsLowerOrEqual()
        {
            // Arrange
            var card = new Card(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), GradeEnum.LimitedRun, 100);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => card.UpgradeGrade(GradeEnum.Factory));
            Assert.Throws<InvalidOperationException>(() => card.UpgradeGrade(GradeEnum.LimitedRun)); // same grade
        }
    }
}
