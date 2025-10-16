namespace LinoVative.Shared.Dto.Bases
{
    public interface IPagination
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
    }

    public abstract class PaginationRequestBase : IPagination
    {
        public int? Page { get; set; } = 1;
        public int? PageSize { get; set; } = 20;
    }
}