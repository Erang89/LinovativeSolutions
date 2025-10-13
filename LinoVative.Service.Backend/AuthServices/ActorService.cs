using LinoVative.Service.Core.Interfaces;

namespace LinoVative.Service.Backend.AuthServices
{
    public class ActorService : IActor, IScoopService
    {
        public ActorService()
        {

        }
        public Guid UserId { get; set; }

        public string EmailAddress { get; set; } = string.Empty;

        public Guid ClientId { get; set; }

        public List<Guid> PrivilegesId { get; set; } = new();

        public string? TimeZone { get; set; }
    }

    public class SystemAdministratorService : ActorService, ISystemAdministrator
    {
        public SystemAdministratorService()
        {
            UserId = new Guid("7d35452e-5aa9-432e-a091-743c6ce7aacf");
            EmailAddress = "system@linovative.com";
        }
    }
}
