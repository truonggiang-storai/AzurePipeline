namespace WebApi.Responses
{
    public class Response<T>
    {        
        public bool Success { get; }
        public string Message { get; }
        public T? Resource { get; }

        public Response(T resource)
        {
            Success = true;
            Message = string.Empty;
            Resource = resource;
        }

        public Response(string message)
        {
            Success = false;
            Message = message;
            Resource = default;
        }
    }
}
