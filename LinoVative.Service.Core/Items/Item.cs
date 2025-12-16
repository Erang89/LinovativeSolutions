using LinoVative.Service.Core.EntityBases;

namespace LinoVative.Service.Core.Items
{
    public class Item : AuditableEntityUnderCompany
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }


        public Guid? UnitId { get; set; }
        public ItemUnit? Unit { get; set; }


        public Guid? CategoryId { get; set; }
        public ItemCategory? Category { get; set; }


        public bool IsActive { get; set; } = true;
        public decimal SellPrice { get; set; }


        public bool SellPriceIncludeTaxService { get; set; }

        public bool HasCostumePrice { get; set; }
        public List<ItemCostumePrice> CostumePrices { get; set; } = new();


        public bool CanBePurchased { get; set; }
    }
}
