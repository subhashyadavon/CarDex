using System;
using CarDexBackend.Domain.Enums;

namespace CarDexBackend.Domain.Entities
{
    public class Card
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }        // Owner
        public Guid VehicleId { get; private set; }     // Vehicle associated with the card
        public Guid CollectionId { get; private set; }  // Collection associated with the card
        public GradeEnum Grade { get; private set; }    // Rarity/grade
        public int Value { get; private set; }          // Current market value

        // Constructor// Domain behavior: update value (e.g., based on market)
        public void UpdateValue(int newValue)
        {
            if (newValue < 0) throw new InvalidOperationException("Value cannot be negative");
            Value = newValue;
        }

        // Domain behavior: upgrade grade
        public void UpgradeGrade(GradeEnum newGrade)
        {
            if ((int)newGrade <= (int)Grade)
                throw new InvalidOperationException("Cannot downgrade or keep the same grade");
            Grade = newGrade;
        }
        public Card(Guid id, Guid userId, Guid vehicleId, Guid collectionId, GradeEnum grade, int value)
        {
            Id = id;
            UserId = userId;
            VehicleId = vehicleId;
            CollectionId = collectionId;
            Grade = grade;
            Value = value;
        }

        
    }
}
