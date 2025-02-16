using Models;
using Context;

namespace Repositories;

public interface IUserRepository
{
    User? GetUserByUsernameAndPassword(string username, string password);
    void CreateUser(User user);
}

public class UserRepository(UserContext context) : IUserRepository
{

    public User? GetUserByUsernameAndPassword(string username, string password)
    {
        // Query the database to find the user by username
        var user = context.Users.SingleOrDefault(user => user.Username == username);
        return user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password) ? null : user;
    }

    public void CreateUser(User user)
    {
        // Hash the password before saving the user
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        context.Users.Add(user);
        context.SaveChanges();
    }
}