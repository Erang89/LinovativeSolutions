using Linovative.Shared.Interface;
using System.ComponentModel.DataAnnotations;

namespace LinoVative.Service.Core.BulkUploads
{
    public class ItemUnitBulkUploadDetail : IEntityId
    {
        [Key]
        public Guid Id { get; set; }
        public ItemUnitBulkUpload? ItemUnitBulkUpload { get; set; }
        public Guid? ItemUnitBulkUploadId { get; set; }

        public string? Column1 { get; set; }
        public string? Column2 { get; set; }
        public string? Errors { get; set; }
    }
}
