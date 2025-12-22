using LinoVative.Service.Core.EntityBases;

namespace LinoVative.Service.Core.Items
{
    public class PriceType : AuditableEntityUnderCompany
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
