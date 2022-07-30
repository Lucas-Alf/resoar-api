using Domain.Entities;

namespace UnitTest.Integration.Repositories.Repositories.DataBuilder
{
    public class UserBuilder
    {
        private User user;
        private List<User> userList;

        public User CreateUser()
        {
            user = new User() { Name = "User from Builder" };
            return user;
        }

        public List<User> CreateUserList(int amount)
        {
            userList = new List<User>();
            for (int i = 0; i < amount; i++)
            {
                userList.Add(CreateUser());
            }

            return userList;
        }
    }
}