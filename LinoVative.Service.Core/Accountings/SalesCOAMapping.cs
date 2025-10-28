using LinoVative.Service.Core.EntityBases;
using LinoVative.Service.Core.Outlets;

namespace LinoVative.Service.Core.Accountings
{
    public class SalesCOAMapping : AuditableEntityUnderCompany
    {
        public Guid OutletId { get; set; }
        public Outlet? Outlet { get; set; }
        public Guid AccountId { get; set; }
        public Account? Account { get; set; }
        public bool AutoPushWhenPOSClosing { get; set; }
    }
}
