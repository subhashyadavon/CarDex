using CarDexBackend.Shared.Dtos.Responses;
using CarDexDatabase;
using Microsoft.EntityFrameworkCore;

namespace CarDexBackend.Services
{
    /// <summary>
    /// Production implementation of <see cref="ICardService"/> using Entity Framework Core and PostgreSQL.
    /// </summary>
    public class CardService : ICardService
    {
        private readonly CarDexDbContext _context;

        public CardService(CarDexDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a list of cards with optional filtering and pagination.
        /// </summary>
        public async Task<CardListResponse> GetAllCards(
            Guid? userId = null,
            Guid? collectionId = null,
            Guid? vehicleId = null,
            string? grade = null,
            int? minValue = null,
            int? maxValue = null,
            string? sortBy = "date_desc",
            int limit = 50,
            int offset = 0)
        {
            var query = _context.Cards.AsQueryable();

            // Apply filters
            if (userId.HasValue)
                query = query.Where(c => c.UserId == userId);

            if (collectionId.HasValue)
                query = query.Where(c => c.CollectionId == collectionId);

            if (vehicleId.HasValue)
                query = query.Where(c => c.VehicleId == vehicleId);

            if (!string.IsNullOrEmpty(grade))
                query = query.Where(c => c.Grade.ToString() == grade);

            if (minValue.HasValue)
                query = query.Where(c => c.Value >= minValue);

            if (maxValue.HasValue)
                query = query.Where(c => c.Value <= maxValue);
            
            //Used AI to help with sorting implementation
            // Apply sorting
            query = sortBy?.ToLower() switch
            {
                "value_asc" => query.OrderBy(c => c.Value),
                "value_desc" => query.OrderByDescending(c => c.Value),
                "grade_asc" => query.OrderBy(c => c.Grade),
                "grade_desc" => query.OrderByDescending(c => c.Grade),
                "date_asc" => query.OrderBy(c => c.CreatedAt),
                "date_desc" => query.OrderByDescending(c => c.CreatedAt),
                _ => query.OrderByDescending(c => c.CreatedAt)
            };

            var total = await query.CountAsync();

            // Join with vehicles to get the name
            var cards = await query
                .Skip(offset)
                .Take(limit)
                .Join(_context.Vehicles,
                    card => card.VehicleId,
                    vehicle => vehicle.Id,
                    (card, vehicle) => new CardResponse
                    {
                        Id = card.Id,
                        Name = $"{vehicle.Year} {vehicle.Make} {vehicle.Model}",
                        Grade = card.Grade.ToString(),
                        Value = card.Value,
                        CreatedAt = card.CreatedAt ?? DateTime.UtcNow
                    })
                .ToListAsync();

            return new CardListResponse
            {
                Cards = cards,
                Total = total,
                Limit = limit,
                Offset = offset
            };
        }

        /// <summary>
        /// Retrieves detailed information about a specific card.
        /// </summary>
        public async Task<CardDetailedResponse> GetCardById(Guid cardId)
        {
            var card = await _context.Cards.FindAsync(cardId);
            if (card == null)
                throw new KeyNotFoundException("Card not found");

            var vehicle = await _context.Vehicles.FindAsync(card.VehicleId);
            var vehicleName = vehicle != null ? $"{vehicle.Year} {vehicle.Make} {vehicle.Model}" : "Unknown Vehicle";

            return new CardDetailedResponse
            {
                Id = card.Id,
                Name = vehicleName,
                Grade = card.Grade.ToString(),
                Value = card.Value,
                CreatedAt = card.CreatedAt ?? DateTime.UtcNow,
                Description = vehicleName,
                VehicleId = card.VehicleId.ToString(),
                CollectionId = card.CollectionId.ToString(),
                OwnerId = card.UserId.ToString()
            };
        }
    }
}
