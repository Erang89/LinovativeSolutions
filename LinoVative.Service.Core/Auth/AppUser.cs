using Linovative.Shared.Interface;
using LinoVative.Service.Core.Interfaces;

namespace LinoVative.Service.Core.Auth
{
    public class AppUser : IEntityId, IAuditableEntity
    {
        public Guid Id { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool HasConfirmed { get; set; }
        public bool IsActive { get; set; }
        public bool ForceChangePasswordOnLogin { get; set; }
        public string? NikName { get; set; }

        public DateTime? CreatedAtUtcTime { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? LastModifiedAtUtcTime { get; set; }

        public Guid? LastModifiedBy { get; set; }

        public void CreateBy(IActor actor)
        {
            CreatedBy = actor.UserId;
            CreatedAtUtcTime = DateTime.UtcNow;
        }

        public void ModifyBy(IActor actor)
        {
            LastModifiedAtUtcTime = DateTime.UtcNow;
            LastModifiedBy = actor.UserId;
        }
    }
}
