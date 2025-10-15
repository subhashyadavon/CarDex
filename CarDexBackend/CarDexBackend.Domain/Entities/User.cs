using System;
using System.Collections.Generic;

namespace CarDexBackend.Domain.Entities
{
    public class User
    {
        // Primary Identity
        public Guid Id { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }  
        public int Currency { get; private set; }

        // Aggregated Entities
        public List<Card> OwnedCards { get; private set; } = new List<Card>();
        public List<Pack> OwnedPacks { get; private set; } = new List<Pack>();
        public List<OpenTrade> OpenTrades { get; private set; } = new List<OpenTrade>();
        public List<CompletedTrade> TradeHistory { get; private set; } = new List<CompletedTrade>();

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

