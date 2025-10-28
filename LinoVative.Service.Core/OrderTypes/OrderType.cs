using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Core.EntityBases;

namespace LinoVative.Service.Core.OrderTypes
{
    public class OrderType : AuditableEntityUnderCompany
    {
        public string? Name { get; set; }
        public OrderBehaviors Behavior { get; set; }
    }
}
