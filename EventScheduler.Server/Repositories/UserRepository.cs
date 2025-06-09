using Microsoft.EntityFrameworkCore;
using EventScheduler.Server.Models;
using EventScheduler.Server.DTOs;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using AutoMapper;

namespace EventScheduler.Server.Repositories
{
    public class UserRepository : IUserRepository
    {
        private SpektraDbContext _context;
        private IMapper _mapper;

        public UserRepository(SpektraDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //is_user//
        public async Task<bool> is_user(string email)
        {
            try
            {
                User? user= await _context.Users.FirstOrDefaultAsync(u=> u.Email == email);
                if (user == null) {
                    return false;
                }
                return true;

            }
            catch (Exception ex) {
                throw ex;
            }
        }


        //get user information with email and password//
        public async Task<User?> get_user_with_email_password(string email, string password)
        {
            try
            {
                User? user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password==password);
                if (user == null)
                {
                    return null;
                }
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //add user//
        public async Task<User> add_user(User user)
        {
            try
            {
                EntityEntry<User> entry = await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                User newUser = entry.Entity;
                return newUser;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                throw;
            }
        }
        //get all users//
        public async Task<List<User>> get_all_users()
        {
            try
            {
                List<User> users = await _context.Users.ToListAsync();
                return users;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }
    }
}
