namespace Linovative.Frontend.Services.Models
{
    public class ResponseBase
    {
        public int Count { get; set; }
        public bool IsValid { get; protected set; } = true;
        public string Title { get; set; } = default!;
        public IDictionary<string, List<string>> Errors { get; set; } = new Dictionary<string, List<string>>();

        public static implicit operator bool(ResponseBase response) => response?.IsValid ?? false;
    }

    public class Response<T> : ResponseBase
    {
        public T? Data { get; set; }

        public Response<T> Result(bool isValid)
        {
            IsValid = isValid;
            return this;
        }

        public static Response<T> Failed(string? message, IDictionary<string, List<string>>? errors = default) => new Response<T>
        {
            IsValid = false,
            Title = message ?? "An error accourred",
            Errors = errors ?? new Dictionary<string, List<string>>()
        };

        public static Response<T> Ok(T? data = default) => new Response<T>
        {
            IsValid = true,
            Data = data
        };

        public static implicit operator bool(Response<T> response) => response?.IsValid ?? false;
    }

    public class Response : Response<bool>
    {
        public Response()
        {
            Data = true;
            IsValid = true;
        }

        public static Response Ok() => new Response
        {
            IsValid = true,
            Data = true
        };

        public static new Response Failed(string? message, IDictionary<string, List<string>>? errors = default) => new Response
        {
            IsValid = false,
            Title = message ?? "An error accourred",
            Errors = errors ?? new Dictionary<string, List<string>>()
        };
    };
}
