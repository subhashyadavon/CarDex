using System;

namespace CarDexBackend.Domain.Entities
{
    public class Vehicle
    {
        public Guid Id { get; set; }
        public string Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }

        // Stats (performance or collectible metrics)
        public int Stat1 { get; set; }
        public int Stat2 { get; set; }
        public int Stat3 { get; set; } 
        public int Value { get; set; } // Market / rarity value

        public string Image { get; set; } // URL or base64

        // Parameterless constructor for EF Core
        public Vehicle()
        {
            Id = Guid.Empty;
            Year = string.Empty;
            Make = string.Empty;
            Model = string.Empty;
            Stat1 = 0;
            Stat2 = 0;
            Stat3 = 0;
            Value = 0;
            Image = string.Empty;
        }

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
