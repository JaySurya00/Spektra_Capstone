using System;
using System.Collections.Generic;

namespace EventScheduler.Server.Models;

public partial class Event
{
    public int Id { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly StartTime { get; set; }

    public string Location { get; set; } = null!;

    public string ImgUrl { get; set; } = null!;

    public int? Capacity { get; set; }

    public string Detail { get; set; } = null!;

    public string EventType { get; set; } = null!;

    public int? TicketCost { get; set; }

    public string? Organizer { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string EventCategories { get; set; } = null!;

    public TimeOnly EndTime { get; set; }

    public string Title { get; set; } = null!;

    public virtual User? OrganizerNavigation { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
