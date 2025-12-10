namespace LinoVative.Shared.Dto.BulkUploads
{
    public class BulkUploadItemDto : EntityDtoBase
    {
        public Guid? UserId { get; set; }
        public string? headerColum1 { get; set; }
        public string? headerColum2 { get; set; }
        public string? headerColum3 { get; set; }
        public string? headerColum4 { get; set; }
        public string? headerColum5 { get; set; }
        public string? headerColum6 { get; set; }
        public string? headerColum7 { get; set; }
        public List<BulkUploadItemDetailDto> Details { get; set; } = new();
    }

    public class BulkUploadItemDetailDto : EntityDtoBase
    {
        public Guid? ItemBulkUploadId { get; set; }
        public string? Column1 { get; set; }
        public string? Column2 { get; set; }
        public string? Column3 { get; set; }
        public string? Column4 { get; set; }
        public string? Column5 { get; set; }
        public string? Column6 { get; set; }
        public string? Column7 { get; set; }
    }
}
