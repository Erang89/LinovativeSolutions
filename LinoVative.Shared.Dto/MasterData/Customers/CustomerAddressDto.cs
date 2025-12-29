using Linovative.Shared.Interface.Enums;
using LinoVative.Shared.Dto.Attributes;
using LinoVative.Shared.Dto.Commons;

namespace LinoVative.Shared.Dto.MasterData.Customers
{
    public class CustomerAddressDto : EntityDtoBase
    {
        public CustomerAddressType AddressType { get; set; }

        [EntityID(EntityTypes.Country)]
        public Guid? CountryId { get; set; }

        public string AddressLine { get; set; } = null!;
        public string? City { get; set; } = null!;
        public string? ProvinceName { get; set; } = null!;
        public string? PostalCode { get; set; }

        public Guid? ProvinceId { get; set; }
    }


    public class CustomerAddressInputDto : CustomerAddressDto
    {
        public IdWithNameDto? Province { get; set; }
    }
}
