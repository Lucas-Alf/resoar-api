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
    }
}
