namespace Domain.Models
{
    public class ResponseMessageModel
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }

        public ResponseMessageModel() { }
        public ResponseMessageModel(string message, object? data = null)
        {
            Success = true;
            Message = message;
            Data = data;
        }

        public ResponseMessageModel(object data)
        {
            Success = true;
            Data = data;
        }
    }
}
