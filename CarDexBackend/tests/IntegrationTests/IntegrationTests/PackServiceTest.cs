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
    public class PackServiceTest : IDisposable
    {
        private readonly CarDexDbContext _context;
        private readonly PackService _packService;
        private readonly IConfiguration _configuration;

        //Used ChatGPT to get the base code and to get help seeding the data
        public PackServiceTest()
        {
            // Set up configuration to read from appsettings.json
            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory) 
                .AddJsonFile("appsettings.json") 
                .Build();

            var connectionString = _configuration.GetConnectionString("CarDexDatabase");

            var options = new DbContextOptionsBuilder<CarDexDbContext>()
                .UseNpgsql(connectionString)  
                .Options;

            _context = new CarDexDbContext(options);
            _packService = new PackService(_context);

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
                Vehicles = new Guid[] { Guid.NewGuid(), Guid.NewGuid() },  // Changed to Guid[] instead of int[]
                PackPrice = 500
            };

            var collection2 = new CarDexBackend.Domain.Entities.Collection
            {
                Id = Guid.NewGuid(),
                Name = "Collection 2",
                Vehicles = new Guid[] { Guid.NewGuid() },  // Changed to Guid[] instead of int[]
                PackPrice = 300
            };

            _context.Collections.Add(collection1);
            _context.Collections.Add(collection2);
            _context.SaveChanges();

            // Add test users
            var user1 = new CarDexBackend.Domain.Entities.User
            {
                Id = Guid.NewGuid(),
                Username = "TestUser",
                Password = "Password123",
                Currency = 1000 
            };

            _context.Users.Add(user1);
            _context.SaveChanges();

            // Add test vehicles
            var vehicle1 = new CarDexBackend.Domain.Entities.Vehicle
            {
                Id = Guid.NewGuid(),  // Changed to Guid instead of int
                Year = "2021",
                Make = "Tesla",
                Model = "Model S",
                Value = 70000
            };

            var vehicle2 = new CarDexBackend.Domain.Entities.Vehicle
            {
                Id = Guid.NewGuid(),  // Changed to Guid instead of int
                Year = "2020",
                Make = "Ford",
                Model = "Mustang",
                Value = 50000
            };

            var vehicle3 = new CarDexBackend.Domain.Entities.Vehicle
            {
                Id = Guid.NewGuid(),  // Changed to Guid instead of int
                Year = "2022",
                Make = "Chevrolet",
                Model = "Camaro",
                Value = 60000
            };

            _context.Vehicles.Add(vehicle1);
            _context.Vehicles.Add(vehicle2);
            _context.Vehicles.Add(vehicle3);
            _context.SaveChanges();
        }
        
        // Test for GetPackById
        [Fact]
        public async Task GetPackById_ShouldReturnCorrectPackDetails()
        {
            // Arrange
            var pack = new CarDexBackend.Domain.Entities.Pack(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                500);  // Changed to Guid instead of int
            _context.Packs.Add(pack);
            _context.SaveChanges();

            // Act
            var result = await _packService.GetPackById(pack.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pack.Id, result.Id);
            Assert.Equal(3, result.PreviewCards.Count()); 
            Assert.Equal(pack.CollectionId, result.CollectionId); 
            Assert.Equal(false, result.IsOpened); 
        }

        // Test for OpenPack
        [Fact]
        public async Task OpenPack_ShouldGenerateCardsAndRemovePack()
        {
            // Arrange
            var pack = new CarDexBackend.Domain.Entities.Pack(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                500);  // Changed to Guid instead of int
            _context.Packs.Add(pack);
            _context.SaveChanges();

            // Act
            var result = await _packService.OpenPack(pack.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Cards.Count()); 
            Assert.Equal(true, result.Pack.IsOpened); 
            Assert.DoesNotContain(pack, _context.Packs); 
        }
    }
}
