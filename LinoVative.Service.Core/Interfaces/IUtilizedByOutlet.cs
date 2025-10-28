using LinoVative.Service.Core.Outlets;

namespace LinoVative.Service.Core.Interfaces
{
    public interface IUtilizedByOutlet
    {
        public Guid? OutletId { get; set; }
        public Outlet? Outlet { get; set; }
    }
}
