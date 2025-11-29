using Linovative.Shared.Interface;
using System.ComponentModel.DataAnnotations;

namespace LinoVative.Service.Core.BulkUploads
{
    public class ItemCategoryBulkUploadDetail : IEntityId
    {
        [Key]
        public Guid Id { get; set; }
        public ItemCategoryBulkUpload? ItemCategoryBulkUpload { get; set; }
        public Guid? ItemCategoryBulkUploadId { get; set; }

        public string? Column1 { get; set; }
        public string? Column2 { get; set; }
    }
}
