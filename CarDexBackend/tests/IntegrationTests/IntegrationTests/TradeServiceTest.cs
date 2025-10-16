using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CarDexBackend.Services;
using CarDexBackend.Shared.Dtos.Responses;
using CarDexBackend.Shared.Dtos.Requests;
using CarDexDatabase;
using CarDexBackend.Domain.Enums;
using Npgsql;
using Xunit;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DefaultNamespace
{
    public class TradeServiceTest : IDisposable
    {
        private readonly CarDexDbContext _context;
        private readonly TradeService _tradeService;

        //Used ChatGPT to get the base code and get help seeding the data, and to write the test for GetOpenTrade with filters.
        public TradeServiceTest()
        {
            // Use In-Memory Database for isolated testing
            var options = new DbContextOptionsBuilder<CarDexDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_TradeService_" + Guid.NewGuid())
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
                Vehicles = new Guid[] { Guid.NewGuid(), Guid.NewGuid() },  
                PackPrice = 500
            };

            var collection2 = new CarDexBackend.Domain.Entities.Collection
            {
                Id = Guid.NewGuid(),
                Name = "Collection 2",
                Vehicles = new Guid[] { Guid.NewGuid() },  
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

            var vehicle3 = new CarDexBackend.Domain.Entities.Vehicle
            {
                Id = Guid.NewGuid(),
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
            
            // Add card for test service user (used by CreateTrade test)
            var testUserId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11");
            var testUserCard = new CarDexBackend.Domain.Entities.Card
            {
                Id = Guid.NewGuid(),
                UserId = testUserId,
                VehicleId = vehicle3.Id,
                CollectionId = collection1.Id,
                Grade = GradeEnum.FACTORY,
                Value = 60000
            };
            _context.Cards.Add(testUserCard);
            _context.SaveChanges();
        }

        // Test for CreateTrade
        [Fact]
        public async Task CreateTrade_ShouldCreateTradeSuccessfully()
        {
            // Arrange - Create a card owned by the test user (TradeService uses hardcoded test user)
            var testUserId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11");
            
            // Create or get the test user
            var testUser = _context.Users.FirstOrDefault(u => u.Id == testUserId);
            if (testUser == null)
            {
                testUser = new CarDexBackend.Domain.Entities.User
                {
                    Id = testUserId,
                    Username = "TestServiceUser",
                    Password = "Password123",
                    Currency = 1000
                };
                _context.Users.Add(testUser);
                _context.SaveChanges();
            }
            
            // Get a card owned by the test user
            var card = _context.Cards.First(c => c.UserId == testUserId);
            
            var request = new TradeCreateRequest
            {
                CardId = card.Id,
                Type = "FOR_PRICE", 
                Price = 1000 
            };

            // Act
            var result = await _tradeService.CreateTrade(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(card.Id, result.CardId); 
            Assert.Equal(request.Price, result.Price); 
            Assert.Equal(testUser.Username, result.Username); 
        }

        // Test for GetOpenTrades with filters
        [Fact]
        public async Task GetOpenTrades_ShouldReturnFilteredOpenTrades()
        {
            // Arrange - Create an open trade first
            var user = _context.Users.First();
            
            // Find a FACTORY card to use
            var factoryCard = _context.Cards.FirstOrDefault(c => c.UserId == user.Id && c.Grade == GradeEnum.FACTORY);
            if (factoryCard == null)
            {
                // Create a FACTORY card if none exists
                var vehicle = _context.Vehicles.First();
                var collection = _context.Collections.First();
                factoryCard = new CarDexBackend.Domain.Entities.Card
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    VehicleId = vehicle.Id,
                    CollectionId = collection.Id,
                    Grade = GradeEnum.FACTORY,
                    Value = 50000
                };
                _context.Cards.Add(factoryCard);
                _context.SaveChanges();
            }
            
            var collectionId = factoryCard.CollectionId;
            
            var openTrade = new CarDexBackend.Domain.Entities.OpenTrade(
                Guid.NewGuid(),
                TradeEnum.FOR_PRICE,
                user.Id,
                factoryCard.Id,
                1000,
                null
            );
            _context.OpenTrades.Add(openTrade);
            _context.SaveChanges();
            
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
            // Arrange - Create an open trade
            var user = _context.Users.First();
            var card = _context.Cards.First(c => c.UserId == user.Id);
            
            var openTrade = new CarDexBackend.Domain.Entities.OpenTrade(
                Guid.NewGuid(),
                TradeEnum.FOR_PRICE,
                user.Id,
                card.Id,
                1000,
                null
            );
            _context.OpenTrades.Add(openTrade);
            _context.SaveChanges();

            // Act
            var result = await _tradeService.GetOpenTradeById(openTrade.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(openTrade.Id, result.Id);
            Assert.Equal(user.Username, result.Username); 
        }

        // Test for ExecuteTrade
        [Fact]
        public async Task ExecuteTrade_ShouldCompleteTheTradeAndTransferCurrency()
        {
            // Arrange - Create buyer (test user) with enough currency
            var testUserId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11");
            var buyer = _context.Users.FirstOrDefault(u => u.Id == testUserId);
            if (buyer == null)
            {
                buyer = new CarDexBackend.Domain.Entities.User
                {
                    Id = testUserId,
                    Username = "BuyerUser",
                    Password = "Password123",
                    Currency = 5000 // Enough to buy
                };
                _context.Users.Add(buyer);
                _context.SaveChanges();
            }
            else
            {
                buyer.Currency = 5000; // Ensure enough currency
                _context.SaveChanges();
            }
            
            // Create seller and their card
            var seller = _context.Users.First(u => u.Id != testUserId);
            var sellerCard = _context.Cards.First(c => c.UserId == seller.Id);
            
            // Create open trade
            var openTrade = new CarDexBackend.Domain.Entities.OpenTrade(
                Guid.NewGuid(),
                TradeEnum.FOR_PRICE,
                seller.Id,
                sellerCard.Id,
                1000,
                null
            );
            _context.OpenTrades.Add(openTrade);
            _context.SaveChanges();
            
            var request = new TradeExecuteRequest
            {
                BuyerCardId = null // FOR_PRICE trade doesn't need buyer card
            };

            // Act
            var result = await _tradeService.ExecuteTrade(openTrade.Id, request);

            // Assert
            Assert.NotNull(result.CompletedTrade);
            Assert.True(result.CompletedTrade.Price > 0); 
            Assert.Equal(seller.Id, result.CompletedTrade.SellerUserId); 
            Assert.Equal(buyer.Id, result.CompletedTrade.BuyerUserId); 
        }
    }
}
