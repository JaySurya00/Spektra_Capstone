using EventScheduler.Server.Models;

namespace EventScheduler.Server.DTOs
{
    public class TicketUpdateDto
    {
        public int TicketId { get; set; }
        public TicketStatus Status { get; set; }
    }
}
