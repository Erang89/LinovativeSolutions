using LinoVative.Service.Core.EntityBases;

namespace LinoVative.Service.Core.Items
{
    public class Item : AuditableEntityUnderCompany
    {
        public string? Name { get; set; }
        public string? Notes { get; set; }
        public Guid CategoryId { get; set; }
        public ItemCategory Category { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public bool HasVariant { get; set; }

        // Selling Settings
        public bool CanBeSell { get; set; }
        public decimal? DefaultSellServicePercent { get; set; }
        public decimal? DefaultSellTaxPercent { get; set; }

        // Purchasing Settings
        public bool CanBePurchased { get; set; }
        public decimal? DefaltPurchaseQty { get; set; }
        public decimal? DefaultMinimumStock { get; set; }

    }
}
