namespace Domain.Models
{
    public class LoginModel
    {
        public string? Email { get; set; }

        public string? Password { get; set; }
    }

    public class RecoverPasswordModel
    {
        public string? Email { get; set; }

        public string? Token { get; set; }
    }
}
