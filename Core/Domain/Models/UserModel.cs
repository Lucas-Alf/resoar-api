namespace Domain.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? ImagePath { get; set; }
    }

    public class UserChangePasswordModel
    {
        public string? OldPassword { get; set; }

        public string? NewPassword { get; set; }
    }
}
