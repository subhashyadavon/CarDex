using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CarDexBackend.Services;
using CarDexBackend.Shared.Dtos.Responses;
using CarDexDatabase;
using Xunit;
using System;
using System.Linq;
using System.Threading.Tasks;
using CarDexBackend.Domain.Enums;

namespace DefaultNamespace
{
    public class TradeServiceTest : IDisposable
    {
        private readonly CarDexDbContext _context;
        private readonly TradeService _tradeService;
        private readonly IConfiguration _configuration;

        //Used ChatGPT to get the base code and get help seeding the data, and to write the test for GetOpenTrade with filters.
        public TradeServiceTest()
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
            _tradeService = new TradeService(_context);

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
                Username = "TestUser1",
                Password = "Password123",
                Currency = 1000 
            };

            var user2 = new CarDexBackend.Domain.Entities.User
            {
                Id = Guid.NewGuid(),
                Username = "TestUser2",
                Password = "Password456",
                Currency = 500 
            };

            _context.Users.Add(user1);
            _context.Users.Add(user2);
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

            // Add test cards
            var card1 = new CarDexBackend.Domain.Entities.Card
            {
                Id = Guid.NewGuid(),
                UserId = user1.Id,
                VehicleId = vehicle1.Id,
                CollectionId = collection1.Id,
                Grade = GradeEnum.FACTORY,
                Value = 70000
            };

            var card2 = new CarDexBackend.Domain.Entities.Card
            {
                Id = Guid.NewGuid(),
                UserId = user2.Id,
                VehicleId = vehicle2.Id,
                CollectionId = collection2.Id,
                Grade = GradeEnum.LIMITED_RUN,
                Value = 50000
            };

            _context.Cards.Add(card1);
            _context.Cards.Add(card2);
            _context.SaveChanges();
        }
        
        
        // Test for GetOpenTrades with filters
        [Fact]
        public async Task GetOpenTrades_ShouldReturnFilteredOpenTrades()
        {
            // Arrange
            var collectionId = _context.Collections.First().Id;
            var grade = "FACTORY"; 
            var minPrice = 500;

            // Act
            var result = await _tradeService.GetOpenTrades(
                type: "FOR_PRICE", 
                collectionId: collectionId, 
                grade: grade, 
                minPrice: minPrice, 
                maxPrice: null, 
                vehicleId: null, 
                wantCardId: null, 
                sortBy: "price_asc", 
                limit: 10, 
                offset: 0
            );

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Trades); 
            Assert.True(result.Trades.All(t => t.Price >= minPrice)); 
            Assert.Equal(10, result.Limit); 
        }

        // Test for GetOpenTradeById
        [Fact]
        public async Task GetOpenTradeById_ShouldReturnCorrectTradeDetails()
        {
            // Arrange
            var tradeId = _context.OpenTrades.First().Id;

            // Act
            var result = await _tradeService.GetOpenTradeById(tradeId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(tradeId, result.Id);
            Assert.Equal("TestUser1", result.Username); 
        }
        
    }
}
