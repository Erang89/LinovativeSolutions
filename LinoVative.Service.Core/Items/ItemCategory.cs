using LinoVative.Service.Core.EntityBases;

namespace LinoVative.Service.Core.Items
{
    public class ItemCategory : AuditableEntityUnderCompany
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Guid? GroupId { get; set; }
        public ItemGroup? Group { get; set; }
    }
}
