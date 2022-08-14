namespace Domain.Models
{
    public class ResponseMessageModel
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }

        public ResponseMessageModel(string message)
        {
            Success = true;
            Message = message;
        }
        public ResponseMessageModel(Exception ex)
        {
            Success = false;
            Message = ex.Message?.TrimEnd('.');

            if (!String.IsNullOrEmpty(ex.InnerException?.Message))
                Message += ". " + ex.InnerException.Message?.TrimEnd('.');
        }

        public ResponseMessageModel(object data)
        {
            Success = true;
            Data = data;
        }
    }
}
