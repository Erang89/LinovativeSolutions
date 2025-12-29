using Linovative.Shared.Interface;

namespace LinoVative.Service.Core.Sources
{
    public class Province : IEntityId
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public Guid? CountryId { get; set; }
    }
}
