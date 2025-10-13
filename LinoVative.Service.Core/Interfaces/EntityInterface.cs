namespace LinoVative.Service.Core.Interfaces
{
   

    public interface IAuditableEntity
    {
        public DateTime? CreatedAtUtcTime { get; }
        public Guid? CreatedBy { get; }
        public DateTime? LastModifiedAtUtcTime { get; }
        public Guid? LastModifiedBy { get; }

        public void ModifyBy(IActor actor);
        public void CreateBy(IActor actor);
    }

    public interface IDeleteableEntity
    {
        public bool IsDeleted { get; }
        public Guid? DeletedBy { get; }
        public DateTime? DeletedAtUTCTime { get;}
        public void Delete(IActor actor);
    }

    public interface IsEntityManageByClinet
    {
        public Guid? ClientId { get; set; }
    }
}
