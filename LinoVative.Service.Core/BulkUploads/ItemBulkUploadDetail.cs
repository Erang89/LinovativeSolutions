using Linovative.Shared.Interface;
using LinoVative.Service.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace LinoVative.Service.Core.BulkUploads
{
    public class ItemBulkUploadDetail : IEntityId, IBuklUploadDetail
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
        public string? Column7 { get; set; }
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
