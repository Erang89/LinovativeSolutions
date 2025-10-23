using Linovative.Shared.Interface;
using LinoVative.Service.Core.Interfaces;

namespace LinoVative.Service.Backend.AuthServices
{
    public class ActorService : IActor, IScoopService
    {
        public ActorService()
        {
            // these are temporary data. 
            // This code should change after authorization implemented
            UserId = new Guid("63CB68D6-D29B-4E65-95DE-6D0769822397");
            CompanyId = new Guid("3FA85F64-5717-4562-B3FC-2C963F66AFA6");
        }
        public Guid UserId { get; set; }

        public string EmailAddress { get; set; } = string.Empty;

        public Guid? CompanyId { get; set; }

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
