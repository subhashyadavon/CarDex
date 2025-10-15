using System;
using CarDexBackend.Domain.Enums;

namespace CarDexBackend.Domain.Entities
{
    public class CompletedTrade
    {
        public Guid Id { get; set; }
        public TradeEnum Type { get; set; }           // ForCard or ForPrice
        public Guid SellerUserId { get; set; }        // Seller
        public Guid SellerCardId { get; set; }        // Card sold by seller
        public Guid BuyerUserId { get; set; }         // Buyer
        public Guid? BuyerCardId { get; set; }        // Card received by seller (if ForCard)
        public int Price { get; set; }                // Price paid (if ForPrice)
        public DateTime? ExecutedDate { get; set; }    // Timestamp of trade completion

        // Timestamps
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Parameterless constructor for EF Core
        public CompletedTrade() { }

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
            if (Type == TradeEnum.FOR_CARD && BuyerCardId == null)
                throw new InvalidOperationException("BuyerCardId must be provided for ForCard trades.");

            if (Type == TradeEnum.FOR_PRICE && Price <= 0)
                throw new InvalidOperationException("Price must be greater than 0 for ForPrice trades.");
        }
    }
}