using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CarDexBackend.Services;
using CarDexBackend.Shared.Dtos.Responses;
using CarDexDatabase;
using CarDexBackend.Domain.Enums;
using Npgsql;
using Xunit;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DefaultNamespace
{
    public class CollectionServiceTest : IDisposable
    {
        private readonly CarDexDbContext _context;
        private readonly CollectionService _collectionService;

        //Used ChatGPT to get the base code and to help seed the data
        public CollectionServiceTest()
        {
            // Use In-Memory Database for isolated testing
            var options = new DbContextOptionsBuilder<CarDexDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_CollectionService_" + Guid.NewGuid())
                .Options;

            _context = new CarDexDbContext(options);
            _collectionService = new CollectionService(_context);

            // Seed test data
            SeedTestData();
        }

        // Dispose method to clean up the DbContext after each test
        public void Dispose()
        {
            _context.Dispose();
        }

        private void SeedTestData()
        {
            // Add test vehicles first
            var vehicle1 = new CarDexBackend.Domain.Entities.Vehicle
            {
                Id = Guid.NewGuid(),
                Year = "2021",
                Make = "Tesla",
                Model = "Model S",
                Value = 70000
            };
            
            var vehicle2 = new CarDexBackend.Domain.Entities.Vehicle
            {
                Id = Guid.NewGuid(),
                Year = "2020",
                Make = "Ford",
                Model = "Mustang",
                Value = 50000
            };
            
            _context.Vehicles.Add(vehicle1);
            _context.Vehicles.Add(vehicle2);
            _context.SaveChanges();
            
            // Add test collections with the actual vehicle IDs
            var collection1 = new CarDexBackend.Domain.Entities.Collection
            {
                Id = Guid.NewGuid(),
                Name = "Collection 1",
                Vehicles = new Guid[] { vehicle1.Id, vehicle2.Id },  
                PackPrice = 500
            };

            var collection2 = new CarDexBackend.Domain.Entities.Collection
            {
                Id = Guid.NewGuid(),
                Name = "Collection 2",
                Vehicles = new Guid[] { vehicle1.Id },  
                PackPrice = 300
            };

            _context.Collections.Add(collection1);
            _context.Collections.Add(collection2);
            _context.SaveChanges();
        }

        // Test for GetAllCollections
        [Fact]
        public async Task GetAllCollections_ShouldReturnCorrectCollections()
        {
            // Act
            var result = await _collectionService.GetAllCollections();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Collections.Count()); 
            Assert.Equal(2, result.Total);
            // Check that both collections exist, don't rely on order
            var collectionNames = result.Collections.Select(c => c.Name).ToList();
            Assert.Contains("Collection 1", collectionNames);
            Assert.Contains("Collection 2", collectionNames);
        }

        // Test for GetCollectionById
        [Fact]
        public async Task GetCollectionById_ShouldReturnCorrectCollection()
        {
            // Arrange - Get Collection 1 specifically
            var collection = _context.Collections.First(c => c.Name == "Collection 1");
            
            // Act
            var result = await _collectionService.GetCollectionById(collection.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(collection.Id, result.Id);
            Assert.Equal("Collection 1", result.Name); 
            // CardCount is based on actual Card entities in the collection, not vehicles array
            // Since we don't create any Card entities in this test, expect 0
            Assert.Equal(0, result.CardCount); 
        }

        // Test for GetCollectionById when collection does not exist
        [Fact]
        public async Task GetCollectionById_ShouldThrowKeyNotFoundException_WhenCollectionNotFound()
        {
            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _collectionService.GetCollectionById(Guid.NewGuid())); 
        }
    }
}
