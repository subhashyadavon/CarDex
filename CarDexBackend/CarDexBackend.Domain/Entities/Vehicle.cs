using System;

namespace CarDexBackend.Domain.Entities
{
    public class Vehicle
    {
        public Guid Id { get; private set; }
        public string Year { get; private set; }
        public string Make { get; private set; }
        public string Model { get; private set; }

        // Stats (performance or collectible metrics)
        public int Stat1 { get; private set; }
        public int Stat2 { get; private set; }
        public int Stat3 { get; private set; } 
        public int Value { get; private set; } // Market / rarity value

        public string Image { get; private set; } // URL or base64

        // Constructor
        public Vehicle(Guid id, string year, string make, string model, int stat1, int stat2, int stat3, int value, string image)
        {
            Id = id;
            Year = year;
            Make = make;
            Model = model;
            Stat1 = stat1;
            Stat2 = stat2;
            Stat3 = stat3;
            Value = value;
            Image = image;
        }
        // Domain Behavior

        // Example: Calculate overall performance rating
        public int CalculateRating()
        {
            return (Stat1 + Stat2 + Stat3) / 3; // simple average for now
        }

        // Update value based on demand or rarity
        public void UpdateValue(int newValue)
        {
            if (newValue < 0) throw new InvalidOperationException("Value cannot be negative");
            Value = newValue;
        }
    }
}
