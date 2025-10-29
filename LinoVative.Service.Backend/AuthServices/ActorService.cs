using IdentityModel;
using Linovative.Shared.Interface;
using LinoVative.Service.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LinoVative.Service.Backend.AuthServices
{
    public class ActorService : IActor, IScoopService
    {
        public ActorService(IHttpContextAccessor httpContextAccessor)
        {

            if (httpContextAccessor?.HttpContext != null)
            {
                var user = httpContextAccessor.HttpContext?.User;
                bool isAuthenticated = user?.Identity?.IsAuthenticated ?? false;

                if (isAuthenticated)
                {
                    UserId = new Guid(user?.Claims.FirstOrDefault(x => x.Properties.FirstOrDefault().Value == JwtClaimTypes.Subject)!.Value!);
                    EmailAddress = user?.Claims.FirstOrDefault(x => x.Properties.FirstOrDefault().Value == JwtClaimTypes.Email)!.Value!;
                    //UserName =  user?.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.PreferredUserName)!.Value!;

                    //if (user?.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.ClientId) is not null)
                    //    ClientId = new Guid(user?.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.ClientId)!.Value!);


                    //Privilages = user?.Claims
                    //    .Where(x => x.Type == AdditionalClaimTypes.Priviliges)
                    //    .Where(x => int.TryParse(x.Value, out _))
                    //    .Where(x => !string.IsNullOrEmpty(PrivilegeNames.GetStringId(Int32.Parse(x.Value))))
                    //    .Select(x => PrivilegeNames.GetStringId(Int32.Parse(x.Value))).ToList() ?? new();
                }
            }
        }
        public Guid UserId { get; set; }

        public string EmailAddress { get; set; } = string.Empty;

        public Guid? CompanyId { get; set; }

        public List<Guid> PrivilegesId { get; set; } = new();

        public string? TimeZone { get; set; }
    }

    public class SystemAdministrator : IActor
    {
        public SystemAdministrator() 
        {
            UserId = new Guid("7d35452e-5aa9-432e-a091-743c6ce7aacf");
            EmailAddress = "system@linovative.com";
        }

        public Guid UserId { get; set; }
        public string EmailAddress { get; set; }

        public Guid? CompanyId { get; set; }

        public List<Guid> PrivilegesId { get; set; } = new();

        public string? TimeZone { get; set; }
    }
}
