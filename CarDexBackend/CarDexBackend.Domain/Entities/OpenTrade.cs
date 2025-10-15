using System;
using CarDexBackend.Domain.Enums;

namespace CarDexBackend.Domain.Entities
{
    public class OpenTrade
    {
        public Guid Id { get; set; }
        public TradeEnum Type { get; set; }
        public Guid UserId { get; set; }       // The user who initiated the trade
        public Guid CardId { get; set; }       // Card offered in the trade
        public int Price { get; set; }         // Used if Type == ForPrice
        public Guid? WantCardId { get; set; }  // Used if Type == ForCard

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Parameterless constructor for EF Core
        public OpenTrade() { }

        // Constructor
        public OpenTrade(Guid id, TradeEnum type, Guid userId, Guid cardId, int price = 0, Guid? wantCardId = null)
        {
            Id = id;
            Type = type;
            UserId = userId;
            CardId = cardId;
            Price = price;
            WantCardId = wantCardId;
            CreatedAt = DateTime.UtcNow;

            ValidateTrade();
        }

        // Domain behavior: validate trade fields
        private void ValidateTrade()
        {
            if (Type == TradeEnum.FOR_CARD && WantCardId == null)
                throw new InvalidOperationException("WantCardId must be provided for ForCard trades.");
            
            if (Type == TradeEnum.FOR_PRICE && Price <= 0)
                throw new InvalidOperationException("Price must be greater than 0 for ForPrice trades.");
        }

        // Update trade price (for ForPrice trades)
        public void UpdatePrice(int newPrice)
        {
            if (Type != TradeEnum.FOR_PRICE) throw new InvalidOperationException("Only ForPrice trades can update price.");
            if (newPrice <= 0) throw new InvalidOperationException("Price must be greater than 0.");
            Price = newPrice;
        }
    }
}