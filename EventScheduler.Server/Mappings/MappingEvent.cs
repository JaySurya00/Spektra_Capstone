using AutoMapper;
using EventScheduler.Server.DTOs;
using EventScheduler.Server.Models;

namespace EventScheduler.Server.Mappings
{
    public class MappingEvent:Profile
    {
        public MappingEvent() {
            CreateMap<EventDto, Event>();
            CreateMap<Event, EventDto>();
        }

    }
}
