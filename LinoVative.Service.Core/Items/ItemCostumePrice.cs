using LinoVative.Service.Core.EntityBases;

namespace LinoVative.Service.Core.Items
{
    public class ItemCostumePrice : AuditableEntity
    {
        public Guid? ItemId { get; set; }
        public Item? Item { get; set; }
        public decimal Price { get; set; }
        public Guid? CostumePriceTagId { get; set; }
        public PriceType? CostumePriceTag { get; set; }
    }
}
