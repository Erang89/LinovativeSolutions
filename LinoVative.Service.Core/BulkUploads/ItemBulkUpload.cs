using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Core.EntityBases;
using LinoVative.Service.Core.Interfaces;

namespace LinoVative.Service.Core.BulkUploads
{
    
    public class ItemBulkUpload : AuditableEntityUnderCompany, IExcelBulkUpload
    {
        public Guid? UserId { get; set; }
        public string? headerColum1 { get; set; }
        public string? headerColum2 { get; set; }
        public string? headerColum3 { get; set; }
        public string? headerColum4 { get; set; }
        public string? headerColum5 { get; set; }
        public string? headerColum6 { get; set; }
        public string? headerColum7 { get; set; }
        public string? headerColum8 { get; set; }
        public string? headerColum9 { get; set; }
        public string? headerColum10 { get; set; }
        public string? headerColum11 { get; set; }
        public string? headerColum12 { get; set; }
        public string? headerColum13 { get; set; }
        public CrudOperations? Operation { get; set; }
        public List<ItemBulkUploadDetail> Details { get; set; } = new();
    }
}
