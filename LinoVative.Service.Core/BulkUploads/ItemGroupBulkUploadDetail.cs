using Linovative.Shared.Interface;
using LinoVative.Service.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace LinoVative.Service.Core.BulkUploads
{
    public class ItemGroupBulkUploadDetail : IEntityId, IBuklUploadDetail
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
            if (!string.IsNullOrWhiteSpace(Errors))
            {
                Errors = $"{Errors}, {error}";
                return;
            }

            Errors = error;
                
        }

        public void ClearError() => Errors = null;
    }
}
