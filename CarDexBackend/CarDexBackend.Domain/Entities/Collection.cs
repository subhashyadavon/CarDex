using System;
using System.Collections.Generic;

namespace CarDexBackend.Domain.Entities
{
    public class Collection
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Image { get; private set; } // URL or base64
        public int PackPrice { get; private set; } // Price for opening a pack
        public List<Vehicle> Vehicles { get; private set; } = new List<Vehicle>();

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
