using Linovative.Shared.Interface;
using System.ComponentModel.DataAnnotations;

namespace LinoVative.Service.Core.BulkUploads
{
    public class ItemGroupBulkUploadDetail : IEntityId
    {
        [Key]
        public Guid Id { get; set; }
        public ItemGroupBulkUpload? ItemGroupBulkUpload { get; set; }
        public Guid? ItemGroupBulkUploadId { get; set; }

        public string? Column1 { get; set; }
    }
}
