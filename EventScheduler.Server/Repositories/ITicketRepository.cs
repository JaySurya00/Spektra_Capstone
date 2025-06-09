using EventScheduler.Server.Models;

namespace EventScheduler.Server.Repositories
{
    public interface ITicketRepository
    {
        Task<List<Ticket>?> get_tickets();
        Task<Ticket?> get_ticket_with_id(int ticket_id);
        Task<Ticket?> add_ticket(Ticket ticket);
        Task<Ticket?> update_ticket_status(int ticketId, TicketStatus new_status);
        Task<List<Ticket>?> get_user_tickets(string user_email);
        Task<List<Ticket>?> get_organizer_tickets(string organizer_email);
        Task<int> event_tickets_count(int event_id);
    }
}
