using Linovative.Shared.Interface;

namespace Linovative.BackendTest.Models
{
    internal class TestActor : IActor
    {
        public Guid UserId { get; set; }

        public string EmailAddress { get; set; } = string.Empty;

        public Guid? CompanyId{ get; set; }

        public List<Guid> PrivilegesId { get; set; } = new();

        public string? TimeZone { get; set; }
    }
}
