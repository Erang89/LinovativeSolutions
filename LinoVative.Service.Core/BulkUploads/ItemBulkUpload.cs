using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Core.EntityBases;

namespace LinoVative.Service.Core.BulkUploads
{
    
    public class ItemBulkUpload : AuditableEntityUnderCompany
    {
        public Guid? UserId { get; set; }
        public string? headerColum1 { get; set; }
        public string? headerColum2 { get; set; }
        public string? headerColum3 { get; set; }
        public string? headerColum4 { get; set; }
        public string? headerColum5 { get; set; }
        public string? headerColum6 { get; set; }
        public string? headerColum7 { get; set; }
        public CrudOperations? Operation { get; set; }
        public List<ItemBulkUploadDetail> Details { get; set; } = new();
    }
}
