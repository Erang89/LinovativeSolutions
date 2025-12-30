using Linovative.Shared.Interface.Enums;
using LinoVative.Shared.Dto.Attributes;
using LinoVative.Shared.Dto.Commons;
using LinoVative.Shared.Dto.Sources;

namespace LinoVative.Shared.Dto.MasterData.Customers
{

    [LocalizerKey(nameof(CustomerDto))]
    public class CustomerAddressDto : EntityDtoBase
    {
        public CustomerAddressType? AddressType { get; set; }

        [EntityID(EntityTypes.Country), LocalizedRequired]
        public Guid? CountryId { get; set; }

        [LocalizedRequired]
        public string AddressLine { get; set; } = null!;
        public string? City { get; set; } = null!;
        public string? ProvinceName { get; set; } = null!;
        public string? PostalCode { get; set; }

        [EntityID(EntityTypes.Province)]
        public Guid? ProvinceId { get; set; }

        [EntityID(EntityTypes.Regency)]
        public Guid? RegencyId { get; set; }
    }


    public class CustomerAddressInputDto : CustomerAddressDto
    {
        public IdWithNameDto? Province { get; set; }
        public IdWithNameDto? Regency { get; set; }
        public CountryDto? Country { get; set; }
    }
}
