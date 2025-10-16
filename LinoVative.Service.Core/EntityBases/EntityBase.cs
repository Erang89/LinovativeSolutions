using Linovative.Shared.Interface;
using LinoVative.Service.Core.Interfaces;

namespace LinoVative.Service.Core.EntityBases
{
    public abstract class AuditableEntity : IEntityId, IAuditableEntity, IDeleteableEntity
    {
        public Guid Id { get; set; }

        public DateTime? CreatedAtUtcTime { get; set; } = DateTime.UtcNow;

        public Guid? CreatedBy {  get; set; }

        public DateTime? LastModifiedAtUtcTime { get; set; }

        public Guid? LastModifiedBy { get; set; }

        public bool IsDeleted { get; set; }

        public Guid? DeletedBy => LastModifiedBy;

        public DateTime? DeletedAtUTCTime => LastModifiedAtUtcTime;

        public void CreateBy(IActor actor)
        {
            CreatedBy = actor.UserId;
            CreatedAtUtcTime = DateTime.UtcNow;
        }


        public void Delete(IActor actor)
        {
            if(IsDeleted) return;

            IsDeleted = true;
            LastModifiedBy = actor.UserId;
            LastModifiedAtUtcTime = DateTime.UtcNow;
        }


        public void ModifyBy(IActor actor)
        {
            LastModifiedBy = actor.UserId;
            LastModifiedAtUtcTime = DateTime.UtcNow;
        }

    }


    public abstract class AuditableEntityUnderCompany : IsEntityManageByCompany, IEntityId
    {
        public Guid? CompanyId { get; set; }
        public Guid Id { get; set; }
    }


}
