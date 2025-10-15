using System;
using System.Collections.Generic;
using CarDexBackend.Domain.Entities;
using Xunit;

namespace CarDexBackend.Tests.UnitTests.Domain.Entities
{
    public class CollectionTests
    {
        [Fact]
        public void Constructor_ShouldInitializePropertiesCorrectly()
        {
            // Arrange
            var id = Guid.NewGuid();
            var name = "Classic Cars";
            var image = "classic.png";
            var packPrice = 500;
            var vehicles = new List<Vehicle>
            {
                new Vehicle(Guid.NewGuid(), "1999", "Toyota", "Supra", 90, 85, 88, 1000, "supra.png")
            };

            // Act
            var collection = new Collection(id, name, image, packPrice, vehicles);

            // Assert
            Assert.Equal(id, collection.Id);
            Assert.Equal(name, collection.Name);
            Assert.Equal(image, collection.Image);
            Assert.Equal(packPrice, collection.PackPrice);
            Assert.Single(collection.Vehicles);
        }

        [Fact]
        public void Constructor_ShouldInitializeEmptyVehicleList_WhenNullIsPassed()
        {
            // Act
            var collection = new Collection(Guid.NewGuid(), "Modern Cars", "modern.png", 400, null);

            // Assert
            Assert.NotNull(collection.Vehicles);
            Assert.Empty(collection.Vehicles);
        }

        [Fact]
        public void AddVehicle_ShouldAddVehicleToList()
        {
            // Arrange
            var collection = new Collection(Guid.NewGuid(), "Super Cars", "super.png", 1000, new List<Vehicle>());
            var vehicle = new Vehicle(Guid.NewGuid(), "2022", "Ferrari", "F8", 99, 98, 100, 2000, "f8.png");

            // Act
            collection.AddVehicle(vehicle);

            // Assert
            Assert.Contains(vehicle, collection.Vehicles);
        }

        [Fact]
        public void AddVehicle_ShouldThrowException_WhenVehicleIsNull()
        {
            // Arrange
            var collection = new Collection(Guid.NewGuid(), "Test", "test.png", 100, new List<Vehicle>());

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => collection.AddVehicle(null));
            Assert.Equal("vehicle", exception.ParamName);
        }

        [Fact]
        public void RemoveVehicle_ShouldRemoveVehicle_WhenExists()
        {
            // Arrange
            var vehicleId = Guid.NewGuid();
            var vehicle = new Vehicle(vehicleId, "2020", "Lamborghini", "Huracan", 95, 96, 97, 3000, "huracan.png");
            var collection = new Collection(Guid.NewGuid(), "Luxury Cars", "lux.png", 800, new List<Vehicle> { vehicle });

            // Act
            collection.RemoveVehicle(vehicleId);

            // Assert
            Assert.Empty(collection.Vehicles);
        }

        [Fact]
        public void HasVehicle_ShouldReturnTrue_WhenVehicleExists()
        {
            // Arrange
            var vehicleId = Guid.NewGuid();
            var vehicle = new Vehicle(vehicleId, "2021", "McLaren", "720S", 97, 98, 99, 3500, "mclaren.png");
            var collection = new Collection(Guid.NewGuid(), "Performance Cars", "perf.png", 900, new List<Vehicle> { vehicle });

            // Act
            var result = collection.HasVehicle(vehicleId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void HasVehicle_ShouldReturnFalse_WhenVehicleDoesNotExist()
        {
            // Arrange
            var collection = new Collection(Guid.NewGuid(), "Vintage Cars", "vintage.png", 300, new List<Vehicle>());

            // Act
            var result = collection.HasVehicle(Guid.NewGuid());

            // Assert
            Assert.False(result);
        }
    }
}
