using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Core.EntityBases;

namespace LinoVative.Service.Core.BulkUploads
{
    
    public class ItemCategoryBulkUpload : AuditableEntityUnderCompany
    {
        public Guid? UserId { get; set; }
        public string? headerColum1 { get; set; }
        public string? headerColum2 { get; set; }
        public string? headerColum3 { get; set; }
        public CrudOperations? Operation { get; set; }
        public List<ItemCategoryBulkUploadDetail> Details { get; set; } = new();
    }
}
