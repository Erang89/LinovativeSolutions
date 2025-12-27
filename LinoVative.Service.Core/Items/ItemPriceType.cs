using LinoVative.Service.Core.EntityBases;

namespace LinoVative.Service.Core.Items
{
    public class ItemPriceType : AuditableEntity
    {
        public Guid? ItemId { get; set; }
        public Item? Item { get; set; }
        public decimal Price { get; set; }
        public Guid? PriceTypeId { get; set; }
        public PriceType? PriceType { get; set; }
        public bool IsActive { get; set; }
    }
}
