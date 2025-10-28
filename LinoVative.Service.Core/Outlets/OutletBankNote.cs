using LinoVative.Service.Core.EntityBases;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Service.Core.Payments;

namespace LinoVative.Service.Core.Outlets
{
    public class OutletBankNote : AuditableEntity, IUtilizedByOutlet
    {
        public bool IsActive { get; set; }
        public Guid? OutletId { get; set; }
        public Guid BankNoteId { get; set; }
        public int Sequence { get; set; }
        public BankNote? BankNote { get; set; }
        public Outlet? Outlet { get; set; }
    }
}
