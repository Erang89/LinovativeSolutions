using Linovative.Shared.Interface;
using LinoVative.Shared.Dto.Attributes;
using System.ComponentModel.DataAnnotations;

namespace LinoVative.Shared.Dto
{

    public class RegisterNewCompanyDto : IEntityId
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [InputRequiredAttribute]
        public string? Name { get; set; }

        [InputRequiredAttribute]
        public string? Address { get; set; }

        [InputRequiredAttribute]
        public Guid? CountryId{ get; set; }

        [InputRequiredAttribute]
        public string? TimeZone { get; set; }

        [InputRequiredAttribute]
        public Guid? CurrencyId { get; set; }

        [InputRequiredAttribute]
        public string? EmailAddress { get; set; }

        [InputRequiredAttribute]
        public string? Password { get; set; }

        [InputRequiredAttribute]
        public string? NickName { get; set; }
    }
}
