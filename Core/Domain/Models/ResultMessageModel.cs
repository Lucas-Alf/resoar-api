namespace Domain.Models
{
    public class ResultMessageModel
    {
        public ResultMessageModel(string message)
        {
            Success = true;
            Message = message;
        }
        public ResultMessageModel(Exception ex)
        {
            Success = false;
            Message = $"{ex.Message?.TrimEnd('.')}. {(ex.InnerException != null ? ex.InnerException.Message : "")}".Trim();
        }

        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}
