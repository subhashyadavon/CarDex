
using System;
using Xunit;
using CarDexBackend.Domain.Entities;
using CarDexBackend.Domain.Enums;

namespace CarDexBackend.Domain.Tests
{
    public class UserTests
    {
        [Fact]
        public void AddCurrency_ShouldIncreaseBalance_WhenAmountIsPositive()
        {
            var user = new User(Guid.NewGuid(), "TestUser", "pass");
            user.AddCurrency(100);

            Assert.Equal(100, user.Currency);
        }
    }
}




