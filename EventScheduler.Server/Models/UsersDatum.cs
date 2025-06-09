using System;
using System.Collections.Generic;

namespace EventScheduler.Server.Models;

public partial class UsersDatum
{
    public string? UserId { get; set; }

    public int? TicketPurchased { get; set; }

    public virtual Ticket? TicketPurchasedNavigation { get; set; }

    public virtual User? User { get; set; }
}
