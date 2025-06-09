using System;
using System.Collections.Generic;

namespace EventScheduler.Server.Models;

public partial class User
{
    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Password { get; set; }

    public DateOnly? Dob { get; set; }

    public string? Role { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
