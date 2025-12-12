using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Core.EntityBases;
using LinoVative.Service.Core.Interfaces;

namespace LinoVative.Service.Core.BulkUploads
{
    
    public class ItemUnitBulkUpload : AuditableEntityUnderCompany, IExcelBulkUpload
    {
        public Guid? UserId { get; set; }
        public string? headerColum1 { get; set; }
        public string? headerColum2 { get; set; }
        public CrudOperations? Operation { get; set; }
        public List<ItemUnitBulkUploadDetail> Details { get; set; } = new();
    }
}
