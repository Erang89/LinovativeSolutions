using System.Net;

namespace LinoVative.Shared.Dto
{
    public class Result
    {
        public string? Title { get; private set; }
        public string? Message { get; private set; } = string.Empty;
        public object? Data { get; private set; }
        public Dictionary<string, object> Errors { get; private set; } = new Dictionary<string, object>();
        public HttpStatusCode Status { get; private set; } = HttpStatusCode.OK;
        public int Count { get; private set; }
        public string? TraceId { get; private set; }

        public void SetTraceId(string traceId) => TraceId = traceId;

        public void AddErrors(Dictionary<string, object> errors)
        {
            foreach (var error in errors)
            {
                Errors.Add(error.Key, error.Value);
            }
        }

        public void AddInvalidProperty(string key, string value)
        {
            if (!Errors.ContainsKey(key))
            {
                Errors.Add(key, new List<string>() { value });
            }

            var existingVal = Errors[key];
            if (existingVal is List<string> listOfString && !listOfString.Contains(value))
            {
                listOfString.Add(value);
                Errors[key] = listOfString;
            }

            Status = HttpStatusCode.BadRequest;
        }

        public void AddInvalidProperty(string key, object value)
        {
            Errors.Add(key, value);
            Status = HttpStatusCode.BadRequest;
        }

        

        public static Result OK(object? data = default, string? message = default, string? title = default)
        {
            return new Result() { Message = message, Title = title };
        }

        public static Result ListOfData(object data, int recordCount)
        {
            return new Result() { Data = data, Count = recordCount };
        }

        public static Result Failed(string message, string? messageTitle = default, Dictionary<string, object>? errorDetails = default, HttpStatusCode? errorCode = default)
        {
            return new Result()
            {
                Message = message,
                Title = messageTitle,
                Errors = errorDetails ?? new(),
                Status = errorCode ?? HttpStatusCode.BadRequest
            };
        }

        public static Result InvalidProperty(string propertyName, string message)
        {
            return new()
            {
                Status = HttpStatusCode.BadRequest,
                Errors = new Dictionary<string, object>() { { propertyName, message } }
            };
        }

        public static implicit operator bool(Result result)
        {
            return result.Status == HttpStatusCode.OK;
        }
    }
}
