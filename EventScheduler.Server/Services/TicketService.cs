using EventScheduler.Server.Models;
using EventScheduler.Server.Repositories;
using Microsoft.AspNetCore.Server.IIS;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace EventScheduler.Server.Services
{
    public class TicketService:ITicketService
    {
        private readonly ITicketRepository _ticket_repo;
        private readonly IEventService _event_srv;
        public TicketService(ITicketRepository ticket_repo, IEventService event_srv)
        {
            _ticket_repo = ticket_repo;
            _event_srv = event_srv;
        }

        //Get all Tickets//

        public async Task<List<Ticket>?> get_tickets()
        {
            return await this._ticket_repo.get_tickets();
        }

        //Ticket with ID//
        public async Task<Ticket?> get_ticket_with_id(int ticket_id)
        {
            try
            {
                Ticket? ticket = await _ticket_repo.get_ticket_with_id(ticket_id);
                return ticket;
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        //Generating Ticket//
        public async Task<Ticket> generate_ticket(int event_id, string owner_email)
        {
            Random rand = new Random();
            try
            {
                Event? event_data = await _event_srv.get_event_with_id(event_id);
                var ticket_id = rand.Next();
                Ticket new_ticket= new Ticket() { Id= ticket_id, Owner= owner_email, EventId=event_id, CreatedAt= DateTime.Today };
                if (event_data?.EventType == EventType.Free.ToString())
                {
                    new_ticket.Status= TicketStatus.Approved.ToString();
                }
                else if(event_data?.EventType == EventType.Invite.ToString())
                {
                    new_ticket.Status = TicketStatus.Pending.ToString();
                }
                else {
                    new_ticket.Status = TicketStatus.Approved.ToString();
                }
                return new_ticket;

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        //Creating Ticket//
        public async Task<List<Ticket>?> create_ticket(int event_id, string owner_email, int count = 1)
        {
            try
            {
                List<Ticket>? tickets = new List<Ticket>();
                for(int i=0; i<count; i++)
                {
                    var ticket = await generate_ticket(event_id, owner_email);
                    var created_ticket= await _ticket_repo.add_ticket(ticket);
                    tickets.Add(created_ticket);
                }
                return tickets;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
       
            }    

        }

        //Update Ticket Status//
        public async Task<Ticket?> update_ticket(int ticket_id, TicketStatus ticketStatus)
        {
            try
            {
                Ticket? ticket= await _ticket_repo.update_ticket_status(ticket_id, ticketStatus);
                return ticket;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //Ticket User Bought//
        public async Task<List<Ticket>?> get_user_ticket_list(string user_emai)
        {
            return await _ticket_repo.get_user_tickets(user_emai);
        }

        //Ticket Organizer Sold//
        public async Task<List<Ticket>?> get_organizer_ticket_list(string organizer)
        {
            return await _ticket_repo.get_organizer_tickets(organizer);
        }

        //Event Ticket count//
        public async Task<int> get_event_ticket_count(int event_id)
        {
            try
            {
                int count= await _ticket_repo.event_tickets_count(event_id);
                return count;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

    }
}
