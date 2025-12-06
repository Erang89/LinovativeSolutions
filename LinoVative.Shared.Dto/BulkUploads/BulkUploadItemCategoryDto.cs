namespace LinoVative.Shared.Dto.BulkUploads
{
    public class BulkUploadItemCategoryDto : EntityDtoBase
    {
        public Guid? UserId { get; set; }
        public string? headerColum1 { get; set; }
        public string? headerColum2 { get; set; }
        public string? headerColum3 { get; set; }
        public List<BulkUploadItemCategoryDetailDto> Details { get; set; } = new();
    }

    public class BulkUploadItemCategoryDetailDto : EntityDtoBase
    {
        public Guid? ItemCategoryBulkUploadId { get; set; }

        public string? Column1 { get; set; }
        public string? Column2 { get; set; }
        public string? Column3 { get; set; }
    }
}
