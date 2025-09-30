using SimpleWpfMvvmAppV8.Data;
using SimpleWpfMvvmAppV8.Models;

namespace SimpleWpfMvvmAppV8.Repositories;

public class UserRepository
{
    public List<User> GetAllUsers()
    {
        using var db = new AppDbContext();
        return [.. db.Users];
    }

    public void AddUser(User user)
    {
        using var db = new AppDbContext();
        db.Users.Add(user);
        db.SaveChanges();
    }
}
