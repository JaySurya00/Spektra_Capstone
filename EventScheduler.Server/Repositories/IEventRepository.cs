using EventScheduler.Server.Models;

namespace EventScheduler.Server.Repositories
{
    public interface IEventRepository
    {
        public Task<List<Event>?> get_all_events();
        Task<Event> add_event(Event eventData);
        Task<Event?> get_event(int event_id);
        Task<List<Event>?> user_events(string userEmail);
        Task update_event(Event eventData);
        Task delete_event(int event_id);
    }
}
