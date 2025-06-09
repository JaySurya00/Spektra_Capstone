using AutoMapper;
using EventScheduler.Server.DTOs;
using EventScheduler.Server.Models;

namespace EventScheduler.Server.Mappings
{
    public class MappingTicket:Profile
    {

        public MappingTicket() {
            CreateMap<Ticket, TicketDto>();
        }

    }
}
