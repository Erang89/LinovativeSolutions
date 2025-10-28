using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Core.EntityBases;

namespace LinoVative.Service.Core.Outlets
{
    public class Outlet : AuditableEntityUnderCompany
    {
        public string? Name { get; set; }
        public OutletTypes? OutletType { get; set; } = OutletTypes.Retail;
        public decimal DefaultTaxPercent { get; set; }
        public decimal DefaultServicePercent { get; set; }

        public List<OutletItemGroup> ItemGroups { get; set; } = new();
        public List<OutletItemCategory> ItemCategories { get; set; } = new();
        public List<OutletTable> Tables { get; set; } = new();
        public List<OutletUser> Users { get; set; } = new();
        public List<OutletArea> Areas { get; set; } = new();
        public List<OutletShift> Shifts { get; set; } = new();
        public List<OutletBankNote> BankNotes { get; set; } = new();
        public List<OutletPaymentMethod> PaymentMethods { get; set; } = new();
        public List<OutletOrderType> OrderTypes { get; set; } = new();
    }
}
