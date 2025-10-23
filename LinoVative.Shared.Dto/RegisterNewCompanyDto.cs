using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;
using LinoVative.Shared.Dto.Attributes;

namespace LinoVative.Shared.Dto
{

    [LocalizerKey(nameof(RegisterNewCompanyDto))]
    public class RegisterNewCompanyDto : IEntityId
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [LocalizedRequired]
        public string? Name { get; set; }

        [LocalizedRequired]
        public string? Address { get; set; }

        [LocalizedRequired, EntityID(EntityTypes.Country)]
        public Guid? CountryId{ get; set; }

        [LocalizedRequired, EntityID(EntityTypes.AppTimeZone)]
        public Guid? TimeZoneId { get; set; }

        [LocalizedRequired, EntityID(EntityTypes.Currency)]
        public Guid? CurrencyId { get; set; }

        [LocalizedRequired]
        public string? EmailAddress { get; set; }

        [LocalizedRequired]
        public string? Password { get; set; }

        [LocalizedRequired]
        public string? NickName { get; set; }
    }
}
