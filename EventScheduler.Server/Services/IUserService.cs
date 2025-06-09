using EventScheduler.Server.DTOs;
using EventScheduler.Server.Models;

namespace EventScheduler.Server.Services
{
    public interface IUserService
    {
        Task<bool> is_user(string email);
        Task<User?> get_user(string email, string password);
        Task<User> signup_user(User user);
        Task<List<User>> get_all_users();
    }
}
