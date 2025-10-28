using LinoVative.Service.Core.EntityBases;

namespace LinoVative.Service.Core.Warehoses
{
    public class Warehouse : AuditableEntityUnderCompany
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
    }
}
