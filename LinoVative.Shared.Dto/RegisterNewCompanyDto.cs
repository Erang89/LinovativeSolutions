using Linovative.Shared.Interface;
using LinoVative.Shared.Dto.Attributes;

namespace LinoVative.Shared.Dto
{

    public class RegisterNewCompanyDto : IEntityId
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [InputRequired]
        public string? Name { get; set; }

        [InputRequired]
        public string? Address { get; set; }

        [InputRequired]
        public Guid? CountryId{ get; set; }

        [InputRequired]
        public string? TimeZone { get; set; }

        [InputRequired]
        public Guid? CurrencyId { get; set; }

        [InputRequired]
        public string? EmailAddress { get; set; }

        [InputRequired]
        public string? Password { get; set; }

        [InputRequired]
        public string? NickName { get; set; }
    }
}
