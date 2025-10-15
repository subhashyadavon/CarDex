using System;
using CarDexBackend.Domain.Enums;

namespace CarDexBackend.Domain.Entities
{
    public class OpenTrade
    {
        public Guid Id { get; private set; }
        public TradeEnum Type { get; private set; }
        public Guid UserId { get; private set; }       // The user who initiated the trade
        public Guid CardId { get; private set; }       // Card offered in the trade
        public int Price { get; private set; }         // Used if Type == ForPrice
        public Guid? WantCardId { get; private set; }  // Used if Type == ForCard

        public DateTime CreatedAt { get; private set; }

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
            if (Type == TradeEnum.ForCard && WantCardId == null)
                throw new InvalidOperationException("WantCardId must be provided for ForCard trades.");
            
            if (Type == TradeEnum.ForPrice && Price <= 0)
                throw new InvalidOperationException("Price must be greater than 0 for ForPrice trades.");
        }

        // Update trade price (for ForPrice trades)
        public void UpdatePrice(int newPrice)
        {
            if (Type != TradeEnum.ForPrice) throw new InvalidOperationException("Only ForPrice trades can update price.");
            if (newPrice <= 0) throw new InvalidOperationException("Price must be greater than 0.");
            Price = newPrice;
        }
    }
}
