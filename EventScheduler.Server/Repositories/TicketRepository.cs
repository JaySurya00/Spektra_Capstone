using EventScheduler.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EventScheduler.Server.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly SpektraDbContext _context;
        public TicketRepository(SpektraDbContext context)
        {
            _context = context;
        }

        //Get Ticket//

        public async Task<List<Ticket>?> get_tickets()
        {
            try
            {
                List<Ticket>? tickets = await _context.Tickets.ToListAsync();
                return tickets;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return null;
            }
        }


        //Get Ticket with ticket_id//
        public async Task<Ticket?> get_ticket_with_id(int ticket_id)
        {
            try
            {
                Ticket? ticket = await _context.Tickets.FindAsync(ticket_id);
                return ticket;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        //Add Ticket to Ticket Table///
        public async Task<Ticket?> add_ticket(Ticket ticket)
        {
            try
            {
                EntityEntry<Ticket> entityEntry = await _context.Tickets.AddAsync(ticket);
                await _context.SaveChangesAsync();
                return entityEntry.Entity;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        //Update Ticket status//
        public async Task<Ticket?> update_ticket_status(int ticket_id, TicketStatus newStatus)
        {
            try
            {
                var ticketIdParam = new SqlParameter("@ticket_id", ticket_id);
                var newStatusParam = new SqlParameter("@new_status", newStatus.ToString());
                await _context.Database.ExecuteSqlRawAsync("Exec sp_update_ticket_status @ticket_id, @new_status", ticketIdParam, newStatusParam);
                Ticket? ticket= await this.get_ticket_with_id(ticket_id);
                return ticket;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }

        }

        //Get users ticket
        public async Task<List<Ticket>?> get_user_tickets(string user_email)
        {
            try
            {
                List<Ticket>? ticket_list= await _context.Tickets.Where(t => t.Owner == user_email).ToListAsync();
                return ticket_list;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        //Organizer ticket sold//
        public async Task<List<Ticket>?> get_organizer_tickets(string organizer_email)
        {
            try
            {
                List<Ticket>? tickets= await _context.Tickets.Where(t => t.Event.Organizer == organizer_email).ToListAsync();
                return tickets;
            }
            catch(Exception ex) {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        //Event Ticket Count//
        public async Task<int> event_tickets_count(int event_id)
        {
            try
            {
                var event_id_param = new SqlParameter("@event_id", event_id);
                var ticket_count_param = new SqlParameter
                {
                    ParameterName = "@ticket_count",                   
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output 
                };

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_event_tickets_count @event_id, @ticket_count OUTPUT",
                    event_id_param,
                    ticket_count_param
                );

                return (int)ticket_count_param.Value;

            }
            catch(Exception ex) {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }
    }
}
