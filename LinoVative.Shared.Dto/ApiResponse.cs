namespace LinoVative.Shared.Dto
{
    public class APIListResponse<T> where T : class
    {
        public int Count { get; set; }
        public List<T> Data { get; set; } = new();
    }

    public class APIDataResponse<T>
    {
        public T? Data { get; set; }
    }

    public class APIInputErrorResponse
    {
        public Dictionary<string, string[]>? Errors { get; set; }
    }

    public class APIInternalErrorResponse
    {
        public string? TraceId { get; private set; }
        public string? Title { get; private set; }
        public string? Message { get; private set; } = string.Empty;
    }
}
