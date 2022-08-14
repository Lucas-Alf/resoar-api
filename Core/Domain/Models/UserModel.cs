namespace Domain.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Image { get; set; }
    }

    public class UserCreateModel
    {
        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? Token { get; set; }
    }

    public class UserUpdateModel
    {
        public string? Name { get; set; }

        public string? Email { get; set; }
    }
}
