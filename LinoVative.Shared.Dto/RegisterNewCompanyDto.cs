using Linovative.Shared.Interface;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LinoVative.Shared.Dto
{

    [DisplayName("Register new company")]
    public class RegisterNewCompanyDto : IEntityId
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "CompanyName is required"), DisplayName("Company name")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Company name is required")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Country is required"), DisplayName("Country")]
        public Guid? CountryId{ get; set; }

        [Required(ErrorMessage = "Time zone is required"), DisplayName("Timezone")]
        public string? TimeZone { get; set; }

        [Required(ErrorMessage = "Currency is required"), DisplayName("Currency")]
        public Guid? CurrencyId { get; set; }


        [Required(ErrorMessage = "Email address is required"), EmailAddress(ErrorMessage = "Invalid email address"), DisplayName("Email address")]
        public string? EmailAddress { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Nikname is required")]
        public string? NickName { get; set; }
    }
}
