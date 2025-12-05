using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Core.EntityBases;

namespace LinoVative.Service.Core.BulkUploads
{
    
    public class ItemUnitBulkUpload : AuditableEntityUnderCompany
    {
        public Guid? UserId { get; set; }
        public string? headerColum1 { get; set; }
        public CrudOperations? Operation { get; set; }
        public List<ItemUnitBulkUploadDetail> Details { get; set; } = new();
    }
}
