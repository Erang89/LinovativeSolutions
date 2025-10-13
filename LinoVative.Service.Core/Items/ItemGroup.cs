using LinoVative.Service.Core.EntityBases;

namespace LinoVative.Service.Core.Items
{
    public class ItemGroup : AuditableEntityUnderCompany
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<ItemCategory> Categories { get; set; } = new();
    }
}
