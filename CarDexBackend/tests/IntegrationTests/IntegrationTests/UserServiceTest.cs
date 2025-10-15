using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CarDexBackend.Services;
using CarDexBackend.Shared.Dtos.Responses;
using CarDexDatabase;
using Xunit;
using System;
using System.Linq;
using System.Threading.Tasks;
using CarDexBackend.Shared.Dtos.Requests;  

namespace DefaultNamespace
{
    public class UserServiceTest : IDisposable
    {
        private readonly CarDexDbContext _context;
        private readonly UserService _userService;
        
        //Used ChatGPT to set up the base code
        public UserServiceTest()
        {
            // Set up configuration to read from appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory) 
                .AddJsonFile("appsettings.json") 
                .Build();
            
            var connectionString = configuration.GetConnectionString("CarDexDatabase");

            var options = new DbContextOptionsBuilder<CarDexDbContext>()
                .UseNpgsql(connectionString) 
                .Options;

            _context = new CarDexDbContext(options);

            // Ensure that the database is up to date with the latest schema
            _context.Database.Migrate();  

            _userService = new UserService(_context);
        }

        // Dispose method to clean up the DbContext
        public void Dispose()
        {
            _context.Dispose();
        }

        // Test for getting user profile by ID
        [Fact]
        public async Task GetUserProfile_ShouldReturnCorrectUser()
        {
            // Arrange: Create a test user in the database
            var user = new CarDexBackend.Domain.Entities.User
            {
                Id = Guid.NewGuid(),  // Use Guid instead of int for Id
                Username = "TestUser",
                Password = "Password123",
                Currency = 100,
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act: Call the service method to get the user profile
            var result = await _userService.GetUserProfile(user.Id);

            // Assert: Check that the user profile returned matches the test user
            Assert.NotNull(result);
            Assert.Equal(user.Id, result.Id);
            Assert.Equal("TestUser", result.Username);
        }

        // Test for updating user profile
        [Fact]
        public async Task UpdateUserProfile_ShouldUpdateUserDetails()
        {
            // Arrange: Create a test user in the database
            var user = new CarDexBackend.Domain.Entities.User
            {
                Id = Guid.NewGuid(),  // Use Guid instead of int for Id
                Username = "TestUser",
                Password = "Password123",
                Currency = 100,
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Update request
            var updateRequest = new CarDexBackend.Shared.Dtos.Requests.UserUpdateRequest
            {
                Username = "UpdatedUser"
            };

            // Act: Call the service method to update the user profile
            var result = await _userService.UpdateUserProfile(user.Id, updateRequest);

            // Assert: Check that the user profile was updated
            Assert.NotNull(result);
            Assert.Equal("UpdatedUser", result.Username);
        }

        // Test for retrieving user cards (assuming cards exist)
        [Fact]
        public async Task GetUserCards_ShouldReturnCorrectCards()
        {
            // Arrange: Create a test user in the database
            var user = new CarDexBackend.Domain.Entities.User
            {
                Id = Guid.NewGuid(),  // Use Guid instead of int for Id
                Username = "TestUser",
                Password = "Password123",
                Currency = 100,
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act: Get the user's cards (empty at the moment)
            var result = await _userService.GetUserCards(user.Id, null, null, 10, 0);

            // Assert: Check the returned result
            Assert.NotNull(result);
            Assert.Equal(0, result.Total); 
        }

        // Test for retrieving user packs
        [Fact]
        public async Task GetUserPacks_ShouldReturnCorrectPacks()
        {
            // Arrange: Create a test user in the database
            var user = new CarDexBackend.Domain.Entities.User
            {
                Id = Guid.NewGuid(),  // Use Guid instead of int for Id
                Username = "TestUser",
                Password = "Password123",
                Currency = 100,
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act: Get the user's packs (empty at the moment)
            var result = await _userService.GetUserPacks(user.Id, null);

            // Assert: Check the returned result
            Assert.NotNull(result);
            Assert.Equal(0, result.Total); 
        }

        [Fact]
        public async Task GetUserTrades_ShouldReturnCorrectTrades()
        {
            // Arrange
            var user = new CarDexBackend.Domain.Entities.User
            {
                Id = Guid.NewGuid(),  // Use Guid instead of int for Id
                Username = "TestUser",
                Password = "Password123",
                Currency = 100,
            };

            var trade = new CarDexBackend.Domain.Entities.OpenTrade
            {
                Id = Guid.NewGuid(),  // Use Guid instead of int for Id
                UserId = user.Id,     // Use Guid for UserId
                Type = CarDexBackend.Domain.Enums.TradeEnum.FOR_CARD,
                CardId = Guid.NewGuid(),
                Price = 500,
            };

            _context.Users.Add(user);
            _context.OpenTrades.Add(trade);
            await _context.SaveChangesAsync();

            // Act
            var result = await _userService.GetUserTrades(user.Id, "ForCard");

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Trades);
            Assert.Equal(trade.Id, result.Trades.First().Id);
            Assert.Equal(trade.Price, result.Trades.First().Price);
        }

        [Fact]
        public async Task GetUserTradeHistory_ShouldReturnCorrectHistory()
        {
            // Arrange
            var user = new CarDexBackend.Domain.Entities.User
            {
                Id = Guid.NewGuid(),  // Use Guid instead of int for Id
                Username = "TestUser",
                Password = "Password123",
                Currency = 100,
            };

            var completedTrade = new CarDexBackend.Domain.Entities.CompletedTrade
            {
                Id = Guid.NewGuid(),  // Use Guid instead of int for Id
                SellerUserId = user.Id,
                BuyerUserId = Guid.NewGuid(),
                Price = 500,
            };

            _context.Users.Add(user);
            _context.CompletedTrades.Add(completedTrade);
            await _context.SaveChangesAsync();

            // Act
            var result = await _userService.GetUserTradeHistory(user.Id, "seller", 10, 0);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Trades);
            Assert.Equal(completedTrade.Id, result.Trades.First().Id);
            Assert.Equal(completedTrade.Price, result.Trades.First().Price);
            Assert.Equal(completedTrade.SellerUserId, result.Trades.First().SellerUserId);
        }

        [Fact]
        public async Task GetUserRewards_ShouldReturnCorrectRewards()
        {
            // Arrange
            var user = new CarDexBackend.Domain.Entities.User
            {
                Id = Guid.NewGuid(),  // Use Guid instead of int for Id
                Username = "TestUser",
                Password = "Password123",
                Currency = 100,
            };

            var reward = new CarDexBackend.Domain.Entities.Reward
            {
                Id = Guid.NewGuid(),  // Use Guid instead of int for Id
                UserId = user.Id,     // Use Guid for UserId
                Type = CarDexBackend.Domain.Enums.RewardEnum.PACK,
                ItemId = Guid.NewGuid(), // Change ItemId to Guid if necessary
                ClaimedAt = null 
            };

            _context.Users.Add(user);
            _context.Rewards.Add(reward);
            await _context.SaveChangesAsync();

            // Act
            var result = await _userService.GetUserRewards(user.Id, false); 

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Rewards);
            Assert.Equal(reward.Id, result.Rewards.First().Id);
            Assert.Equal(reward.Type.ToString(), result.Rewards.First().Type);
            Assert.Null(result.Rewards.First().ClaimedAt); 
        }

    }
}
