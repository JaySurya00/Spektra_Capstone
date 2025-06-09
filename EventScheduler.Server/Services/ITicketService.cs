using EventScheduler.Server.Models;

namespace EventScheduler.Server.Services
{
    public interface ITicketService
    {
        Task<List<Ticket>?> get_tickets();
        Task<Ticket?> get_ticket_with_id(int ticket_id);
        Task<List<Ticket>?> create_ticket(int event_id, string owner_email, int count=1);
        Task<Ticket> generate_ticket(int event_id, string owner_email);
        Task<Ticket?> update_ticket(int ticket_id, TicketStatus ticketStatus);
        Task<List<Ticket>?> get_user_ticket_list(string user_email);
        Task<List<Ticket>?> get_organizer_ticket_list(string organizer);
        Task<int> get_event_ticket_count(int event_id);

    }
}
