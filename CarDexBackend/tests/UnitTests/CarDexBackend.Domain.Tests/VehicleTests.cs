using System;
using CarDexBackend.Domain.Entities;
using Xunit;

namespace CarDexBackend.Tests.UnitTests.Domain.Entities
{
    public class VehicleTests
    {
        [Fact]
        public void Constructor_ShouldInitializeAllPropertiesCorrectly()
        {
            // Arrange
            var id = Guid.NewGuid();
            string year = "2022";
            string make = "Toyota";
            string model = "Supra";
            int stat1 = 80;
            int stat2 = 90;
            int stat3 = 100;
            int value = 50000;
            string image = "https://example.com/car.png";

            // Act
            var vehicle = new Vehicle(id, year, make, model, stat1, stat2, stat3, value, image);

            // Assert
            Assert.Equal(id, vehicle.Id);
            Assert.Equal(year, vehicle.Year);
            Assert.Equal(make, vehicle.Make);
            Assert.Equal(model, vehicle.Model);
            Assert.Equal(stat1, vehicle.Stat1);
            Assert.Equal(stat2, vehicle.Stat2);
            Assert.Equal(stat3, vehicle.Stat3);
            Assert.Equal(value, vehicle.Value);
            Assert.Equal(image, vehicle.Image);
        }

        [Fact]
        public void CalculateRating_ShouldReturnAverageOfStats()
        {
            // Arrange
            var vehicle = new Vehicle(Guid.NewGuid(), "2023", "Tesla", "Model S", 90, 80, 70, 80000, "img");

            // Act
            var rating = vehicle.CalculateRating();

            // Assert
            Assert.Equal((90 + 80 + 70) / 3, rating);
        }

        [Fact]
        public void UpdateValue_ShouldChangeValue_WhenNewValueIsValid()
        {
            // Arrange
            var vehicle = new Vehicle(Guid.NewGuid(), "2021", "Ford", "Mustang", 70, 75, 80, 40000, "img");

            // Act
            vehicle.UpdateValue(45000);

            // Assert
            Assert.Equal(45000, vehicle.Value);
        }

        [Fact]
        public void UpdateValue_ShouldThrowException_WhenNewValueIsNegative()
        {
            // Arrange
            var vehicle = new Vehicle(Guid.NewGuid(), "2021", "Ford", "Mustang", 70, 75, 80, 40000, "img");

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => vehicle.UpdateValue(-1000));
        }
    }
}
