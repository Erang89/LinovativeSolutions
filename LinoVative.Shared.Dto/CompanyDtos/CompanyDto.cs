using Linovative.Shared.Interface;
using LinoVative.Shared.Dto.Commons;

namespace LinoVative.Shared.Dto.CompanyDtos
{
    public class CompanyDto : IEntityId
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }

        public Guid? CountryId { get; set; }
        public IdWithCodeDto? Country { get; set; }
        public Guid? TimeZoneId { get; set; }
        public Guid? OwnByUserId { get; set; }
        public Guid? CurrencyId { get; set; }
        public IdWithCodeDto? Currency { get; set; }
    }
}
