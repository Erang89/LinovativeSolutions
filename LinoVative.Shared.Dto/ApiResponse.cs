namespace LinoVative.Shared.Dto
{
    public class APIListResponse<T> where T : class
    {
        public int Count { get; set; }
        public List<T> Data { get; set; } = new();
    }

    public class APIInputErrorResponse
    {
        public string? TraceId { get; private set; }
        public Dictionary<string, object>? Errors { get; set; }
    }

    public class APIInternalErrorResponse
    {
        public string? Title { get; private set; }
        public string? Message { get; private set; } = string.Empty;
    }
}
