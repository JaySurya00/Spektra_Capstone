

using AutoMapper;
using EventScheduler.Server.DTOs;
using EventScheduler.Server.Models;
using EventScheduler.Server.Repositories;

namespace EventScheduler.Server.Services
{
    public class UserService:IUserService
    {
        private IUserRepository _userRepo;
        private IMapper _mapper;

        public UserService(IUserRepository userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        //check user exist//
        public async Task<bool> is_user(string email)
        {
            try
            {
                bool is_user = await _userRepo.is_user(email);
                return is_user;
            }
            catch (Exception ex) {
                throw;
            }
        }

        //get user data//
        public async Task<User?> get_user(string email, string password)
        {
            try
            {
                User? user= await _userRepo.get_user_with_email_password(email, password);
                return user;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return null;
            }
        }

        //signup user//
        public async Task<User> signup_user(User newUser)
        {
            try
            {
                User newCreateUser = await _userRepo.add_user(newUser);
                return newUser;
            }
            catch (Exception ex) {
                throw;
            }
        }

        //get all users//
        public async Task<List<User>> get_all_users()
        {
            try
            {
                List<User> users = await _userRepo.get_all_users();
                return users;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
