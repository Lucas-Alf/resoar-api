namespace Domain.Models
{
    public class ErrorMessageModel : ResponseMessageModel
    {
        public ErrorMessageModel(string message, object data)
        {
            Success = false;
            Message = message;
            Data = data;
        }

        public ErrorMessageModel(Exception ex)
        {
            Success = false;
            Message = ex.Message?.TrimEnd('.');

            if (!String.IsNullOrEmpty(ex.InnerException?.Message))
                Message += ". " + ex.InnerException.Message?.TrimEnd('.');
        }
    }
}
