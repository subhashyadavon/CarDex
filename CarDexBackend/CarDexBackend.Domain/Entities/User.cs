using System;
using System.Collections.Generic;

namespace CarDexBackend.Domain.Entities
{
    public class User
    {
        // Primary Identity
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }  
        public int Currency { get; set; }

        // Aggregated Entities
        public List<Card> OwnedCards { get; set; } = new List<Card>();
        public List<Pack> OwnedPacks { get; set; } = new List<Pack>();
        public List<OpenTrade> OpenTrades { get; set; } = new List<OpenTrade>();
        public List<CompletedTrade> TradeHistory { get; set; } = new List<CompletedTrade>();

        // Timestamps
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Parameterless constructor for EF Core
        public User() { }

        // Constructor
        public User(Guid id, string username, string password)
        {
            Id = id;
            Username = username;
            Password = password;
            Currency = 0;
        }

        // Domain Behaviors

        // Currency management
        public void AddCurrency(int amount)
        {
            if (amount < 0) throw new InvalidOperationException("Amount cannot be negative");
            Currency += amount;
        }

        public void DeductCurrency(int amount)
        {
            if (amount > Currency) throw new InvalidOperationException("Insufficient currency");
            Currency -= amount;
        }

        // Card management
        public void AddCard(Card card) => OwnedCards.Add(card);
        public bool HasCard(Guid cardId) => OwnedCards.Exists(c => c.Id == cardId);

        // Pack management
        public void AddPack(Pack pack) => OwnedPacks.Add(pack);

        // Trade management
        public void AddOpenTrade(OpenTrade trade) => OpenTrades.Add(trade);
        public void CompleteTrade(CompletedTrade trade)
        {
            TradeHistory.Add(trade);
            OpenTrades.RemoveAll(t => t.Id == trade.Id);
        }
    }
}