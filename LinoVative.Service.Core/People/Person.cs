using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Core.EntityBases;

namespace LinoVative.Service.Core.People
{
    public class Person : AuditableEntityUnderCompany
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Nikname { get; set; }
        public PersonTitles? Title { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
