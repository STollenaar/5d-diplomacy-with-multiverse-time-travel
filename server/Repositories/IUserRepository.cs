using Models;

namespace Repositories;

public interface IUserRepository
{
    User GetUserByUsernameAndPassword(string username, string password);
}

public class UserRepository : IUserRepository
{
    // This is just a placeholder. Replace with actual implementation.
    public User GetUserByUsernameAndPassword(string username, string password) => new(id: 1, username, password);

}