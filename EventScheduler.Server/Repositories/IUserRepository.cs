using EventScheduler.Server.Models;
using EventScheduler.Server.DTOs;


namespace EventScheduler.Server.Repositories;

public interface IUserRepository
{
    Task<User?> get_user_with_email_password(string email, string password);
    Task<bool> is_user(string email);
    Task<User> add_user(User user);
    Task<List<User>> get_all_users();
}
