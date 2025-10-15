using System;
using CarDexBackend.Domain.Enums;

namespace CarDexBackend.Domain.Entities
{
    public class CompletedTrade
    {
        public Guid Id { get; private set; }
        public TradeEnum Type { get; private set; }           // ForCard or ForPrice
        public Guid SellerUserId { get; private set; }        // Seller
        public Guid SellerCardId { get; private set; }        // Card sold by seller
        public Guid BuyerUserId { get; private set; }         // Buyer
        public Guid? BuyerCardId { get; private set; }        // Card received by seller (if ForCard)
        public int Price { get; private set; }                // Price paid (if ForPrice)
        public DateTime ExecutedDate { get; private set; }    // Timestamp of trade completion

        // Constructor
        public CompletedTrade(
            Guid id,
            TradeEnum type,
            Guid sellerUserId,
            Guid sellerCardId,
            Guid buyerUserId,
            int price = 0,
            Guid? buyerCardId = null)
        {
            Id = id;
            Type = type;
            SellerUserId = sellerUserId;
            SellerCardId = sellerCardId;
            BuyerUserId = buyerUserId;
            Price = price;
            BuyerCardId = buyerCardId;
            ExecutedDate = DateTime.UtcNow;

            ValidateTrade();
        }

        // Domain validation
        private void ValidateTrade()
        {
            if (Type == TradeEnum.ForCard && BuyerCardId == null)
                throw new InvalidOperationException("BuyerCardId must be provided for ForCard trades.");

            if (Type == TradeEnum.ForPrice && Price <= 0)
                throw new InvalidOperationException("Price must be greater than 0 for ForPrice trades.");
        }
    }
}
