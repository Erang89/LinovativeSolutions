using Linovative.Shared.Interface;
using Linovative.Shared.Interface.Enums;

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

    public interface IsEntityManageByCompany
    {
        public Guid? CompanyId { get; set; }
    }

    public interface IEntityManageByUser
    {
        public Guid? UserId { get; set; }
    }

    public interface IExcelBulkUpload : IEntityManageByUser, IsEntityManageByCompany
    {
        CrudOperations? Operation { get; set; }
    }
}
