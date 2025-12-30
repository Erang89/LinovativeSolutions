using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Core.EntityBases;

namespace LinoVative.Service.Core.People
{
    public class Person : AuditableEntityUnderCompany
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Nikname { get; set; }
        public PersonTitles? Title { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
