using LinoVative.Service.Core.EntityBases;

namespace LinoVative.Service.Core.Items
{
    public class SKUItem : AuditableEntityUnderCompany
    {
        public Guid ItemId { get; set; }
        public Item Item { get; set; } = null!;

        public string SKU { get; set; } = null!;
        public string VarianName { get; set; } = null!;

        public Guid UnitId { get; set; }
        public ItemUnit Unit { get; set; } = null!;

        public Guid CategoryId { get; set; }
        public ItemCategory Category { get; set; } = null!;

        public decimal SalePrice { get; set; }

        // Sale Tax and service
        public bool HasSaleTaxAndService { get; set; }
        public bool SalePriceIncludeTaxAndService { get; set; }
        public bool UseOutletSaleTaxAndService { get; set; }

        // Purchase Settings
        public decimal DefaultPurchaseQty { get; set; }
        public decimal MinimumStockQty { get; set; }

        // Costume Price
        public bool HasCostumePrice { get; set; }
        public ICollection<ItemPriceType> PriceTypes { get; set; } = new List<ItemPriceType>();
    }
}
