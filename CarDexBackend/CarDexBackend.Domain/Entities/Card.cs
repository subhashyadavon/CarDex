using System;
using CarDexBackend.Domain.Enums;

namespace CarDexBackend.Domain.Entities
{
    public class Card
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }        // Owner
        public Guid VehicleId { get; set; }     // Vehicle associated with the card
        public Guid CollectionId { get; set; }  // Collection associated with the card
        public GradeEnum Grade { get; set; }    // Rarity/grade
        public int Value { get; set; }          // Current market value

        // Timestamps
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Parameterless constructor for EF Core
        public Card() { }

        // Constructor
        public Card(Guid id, Guid userId, Guid vehicleId, Guid collectionId, GradeEnum grade, int value)
        {
            Id = id;
            UserId = userId;
            VehicleId = vehicleId;
            CollectionId = collectionId;
            Grade = grade;
            Value = value;
        }

        // Domain behavior: update value (e.g., based on market)
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
    }
}