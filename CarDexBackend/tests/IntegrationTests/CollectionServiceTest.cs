using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CarDexBackend.Services;
using CarDexBackend.Shared.Dtos.Responses;
using CarDexDatabase;
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
        private readonly IConfiguration _configuration;

        //Used ChatGPT to get the base code and to help seed the data
        public CollectionServiceTest()
        {
            // Set up configuration to read from appsettings.json
            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory) 
                .AddJsonFile("appsettings.json") 
                .Build();
            
            var connectionString = _configuration.GetConnectionString("SupabaseConnection");

            
            var options = new DbContextOptionsBuilder<CarDexDbContext>()
                .UseNpgsql(connectionString)  
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
            // Add test collections 
            var collection1 = new CarDexBackend.Domain.Entities.Collection
            {
                Id = Guid.NewGuid(),
                Name = "Collection 1",
                Vehicles = new int[] { 1, 2 },  
                PackPrice = 500
            };

            var collection2 = new CarDexBackend.Domain.Entities.Collection
            {
                Id = Guid.NewGuid(),
                Name = "Collection 2",
                Vehicles = new int[] { 3 },  
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
            Assert.Equal(2, result.Collections.Count); 
            Assert.Equal(2, result.Total); 
            Assert.Equal("Collection 1", result.Collections.First().Name);
            Assert.Equal("Collection 2", result.Collections.Last().Name);
        }

        // Test for GetCollectionById
        [Fact]
        public async Task GetCollectionById_ShouldReturnCorrectCollection()
        {
            // Arrange
            var collection = _context.Collections.First();
            
            // Act
            var result = await _collectionService.GetCollectionById(collection.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(collection.Id, result.Id);
            Assert.Equal("Collection 1", result.Name); 
            Assert.Equal(2, result.CardCount); 
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
