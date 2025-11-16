using LinoVative.Service.Core.EntityBases;

namespace LinoVative.Service.Core.BulkUploads
{
    public class ItemGroupBulkUpload : AuditableEntityUnderCompany
    {
        public Guid? UserId { get; set; }
        public string? headerColum1 { get; set; }
        public string? headerColum2 { get; set; }
    }
}
