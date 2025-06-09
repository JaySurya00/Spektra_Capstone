using EventScheduler.Server.Models;
using Microsoft.IdentityModel.Tokens;

namespace EventScheduler.Server.Repositories
{
    public class UserDataRepository:IUserDataRepository
    {
        private readonly SpektraDbContext _context;
        public UserDataRepository(SpektraDbContext context) {
            _context = context;
        }

        //Assign ticket to user and save info to usersData DB//
        public async Task<bool> assign_ticket_to_user(string user_email, int ticket_id)
        {
            try
            {
                UsersDatum user_data = new UsersDatum() { UserId = user_email, TicketPurchased = ticket_id };
                await _context.UsersData.AddAsync(user_data);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}
