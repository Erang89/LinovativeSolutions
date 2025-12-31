using Linovative.Dto.MasterData.People;
using Linovative.Shared.Interface.Enums;
using LinoVative.Shared.Dto.Attributes;

namespace LinoVative.Shared.Dto.MasterData.Customers
{

    [LocalizerKey(nameof(CustomerDto))]
    public class CustomerDto : EntityDtoBase
    {
        [EntityID(EntityTypes.Person)]
        public Guid? PersonId { get; set; }
        public string? CustomerCode { get; set; }
        public string? Name { get; set; }
        public string? LegalName { get; set; }

        public CustomerType? CustomerType { get; set; }

        public bool IsMember { get; set; }
        public string? MemberNumber { get; set; }
        public int Points { get; set; }

        public string? TaxNumber { get; set; }
        public decimal? CreditLimit { get; set; }
        public bool AllowCredit { get; set; }
        public string? PaymentTerm { get; set; }

        public bool IsActive { get; set; } = true;
    }


    public class CustomerInputDto : CustomerDto
    {
        public List<CustomerAddressInputDto> Address { get; set; } = new();
        public List<CustomerContactDto> Contacts { get; set; } = new();
        public PersonDto? Person { get; set; }
    }


    public class CustomerViewDto : CustomerDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
