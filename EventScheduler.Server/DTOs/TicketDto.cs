namespace EventScheduler.Server.DTOs
{
    public class TicketDto
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string Owner { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
