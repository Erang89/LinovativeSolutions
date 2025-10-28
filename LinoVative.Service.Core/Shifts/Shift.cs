using LinoVative.Service.Core.EntityBases;

namespace LinoVative.Service.Core.Shifts
{
    public class Shift : AuditableEntityUnderCompany
    {
        public string? Name { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
    }
}
