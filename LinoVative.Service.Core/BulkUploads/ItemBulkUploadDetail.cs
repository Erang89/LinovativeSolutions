using Linovative.Shared.Interface;
using System.ComponentModel.DataAnnotations;

namespace LinoVative.Service.Core.BulkUploads
{
    public class ItemBulkUploadDetail : IEntityId
    {
        [Key]
        public Guid Id { get; set; }
        public ItemBulkUpload? ItemBulkUpload { get; set; }
        public Guid? ItemBulkUploadId { get; set; }

        public string? Column1 { get; set; }
        public string? Column2 { get; set; }
        public string? Column3 { get; set; }
        public string? Column4 { get; set; }
        public string? Column5 { get; set; }
        public string? Column6 { get; set; }
    }
}
