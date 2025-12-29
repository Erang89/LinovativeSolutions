using Linovative.Shared.Interface;

namespace LinoVative.Service.Core.Sources
{
    public class Regency : IEntityId
    {
        public Guid ProvinceId { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public Guid Id { get; set; }
    }
}
