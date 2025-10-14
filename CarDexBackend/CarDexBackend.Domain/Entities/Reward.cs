using System;
using CarDexBackend.Domain.Enums;

namespace CarDexBackend.Domain.Entities
{
    public class Reward
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public RewardEnum Type { get; private set; }
        public Guid? ItemId { get; private set; } 
        public int Amount { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public DateTime? ClaimedAt { get; private set; } // Null until claimed

        // Constructor
        public Reward(Guid id, Guid userId, RewardEnum type, int amount, Guid? itemId = null)
        {
            Id = id;
            UserId = userId;
            Type = type;
            Amount = amount;
            ItemId = itemId;
            CreatedAt = DateTime.UtcNow;
            ClaimedAt = null;
        }

        // Domain behavior: claim reward
        public void Claim()
        {
            if (ClaimedAt != null) throw new InvalidOperationException("Reward already claimed");
            ClaimedAt = DateTime.UtcNow;
        }

        // Check if reward is already claimed
        public bool IsClaimed() => ClaimedAt != null;
    }
}
