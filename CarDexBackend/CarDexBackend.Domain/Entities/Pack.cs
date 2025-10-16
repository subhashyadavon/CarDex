using System;

namespace CarDexBackend.Domain.Entities
{
    public class Pack
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }        // Owner
        public Guid CollectionId { get; set; }  // Collection this pack belongs to
        public int Value { get; set; }          // Current value of the pack

        // Parameterless constructor for EF Core
        public Pack()
        {
            Id = Guid.Empty;
            UserId = Guid.Empty;
            CollectionId = Guid.Empty;
            Value = 0;
        }

        // Constructor
        public Pack(Guid id, Guid userId, Guid collectionId, int value)
        {
            Id = id;
            UserId = userId;
            CollectionId = collectionId;
            Value = value;
        }

        // Domain behavior: update value (e.g., due to market or rarity)
        public void UpdateValue(int newValue)
        {
            if (newValue < 0) throw new InvalidOperationException("Value cannot be negative");
            Value = newValue;
        }
    }
}
