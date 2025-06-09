using EventScheduler.Server.DTOs;
using EventScheduler.Server.Models;

namespace EventScheduler.Server.Services
{
    public interface IEventService
    {
        Task<Event> create_event(Event new_event_data);
        Task<List<Event>?> get_all_events();
        Task<Event?> get_event_with_id(int event_id);
        Task<List<Event>?> get_my_events(string user_email);
        Task update_event(Event new_event_data);
        Task<string> delete_event(int event_id);
    }
}
