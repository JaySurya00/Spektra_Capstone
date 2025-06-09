using System;
using System.Collections.Generic;

namespace EventScheduler.Server.Models;

public partial class Ticket
{
    public int Id { get; set; }

    public int EventId { get; set; }

    public string Owner { get; set; } = null!;

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual User OwnerNavigation { get; set; } = null!;
}
