namespace Domain.Models
{
    public class ResultMessageModel
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }

        public ResultMessageModel(string message)
        {
            Success = true;
            Message = message;
        }
        public ResultMessageModel(Exception ex)
        {
            Success = false;
            Message = ex.Message?.TrimEnd('.');

            if (!String.IsNullOrEmpty(ex.InnerException?.Message))
                Message += ". " + ex.InnerException.Message?.TrimEnd('.');
        }

        public ResultMessageModel(object data)
        {
            Success = true;
            Data = data;
        }
    }
}
