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
        public string? Column2 { get; set; }
        public string? Errors { get; set; }

        public void AddError(string error)
        {
            if (!string.IsNullOrWhiteSpace(error))
            {
                Errors = $"{Errors}, {error}";
                return;
            }

            Errors = error;
                
        }
    }
}
