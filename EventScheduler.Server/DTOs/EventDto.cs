namespace EventScheduler.Server.DTOs
{
    public class EventDto
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string Location { get; set; }
        public string Detail { get; set; }
        public Int32 Capacity { get; set; }
        public string ImgUrl { get; set; }
        public string EventCategories { get; set; }
        public string EventType { get; set; }
        public int TicketCost { get; set; }
        public string? Organizer { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool is_deleted { get; set; }
    }
}
