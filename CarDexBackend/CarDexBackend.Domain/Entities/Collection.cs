using System;
using System.Collections.Generic;

namespace CarDexBackend.Domain.Entities
{
    public class Collection
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; } // URL or base64
        public int PackPrice { get; set; } // Price for opening a pack
        public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();

        // Timestamps
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Parameterless constructor for EF Core
        public Collection() { }

        // Constructor
        public Collection(Guid id, string name, string image, int packPrice, List<Vehicle> vehicles)
        {
            Id = id;
            Name = name;
            Image = image;
            PackPrice = packPrice;
            Vehicles = vehicles ?? new List<Vehicle>();
        }

       // Domain Behavior

        // Add a vehicle to the collection
        public void AddVehicle(Vehicle vehicle)
        {
            if (vehicle == null) throw new ArgumentNullException(nameof(vehicle));
            Vehicles.Add(vehicle);
        }

        // Remove a vehicle from the collection
        public void RemoveVehicle(Guid vehicleId)
        {
            Vehicles.RemoveAll(v => v.Id == vehicleId);
        }

        // Check if collection has a specific vehicle
        public bool HasVehicle(Guid vehicleId) => Vehicles.Exists(v => v.Id == vehicleId);
    }
}