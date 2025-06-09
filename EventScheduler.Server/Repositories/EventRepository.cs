using EventScheduler.Server.DTOs;
using EventScheduler.Server.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace EventScheduler.Server.Repositories
{
    public class EventRepository: IEventRepository
    {
        private readonly SpektraDbContext _context;

        public EventRepository(SpektraDbContext context)
        {
            _context = context;
        }

        //Get all Events From DB//
        public async Task<List<Event>?> get_all_events()
        {
            List<Event> events= await _context.Events.ToListAsync();
            return events;
        }

        //Add new Event to DB//
        public async Task<Event> add_event(Event eventData){
            try
            {
                EntityEntry<Event> entry = await _context.Events.AddAsync(eventData);
                await _context.SaveChangesAsync();
                Event created_event = entry.Entity;
                return created_event;
            }
            catch (Exception ex) {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        //Get Event with id//
        public async Task<Event?> get_event(int id)
        {
            try
            {
                Event? event_data = await _context.Events.FindAsync(id);
                return event_data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        //Get current user event from DB//
        public async Task<List<Event>?> user_events(string user_email)
        {
            try
            {
                List<Event>? events = await _context.Events.Where(e=>e.Organizer==user_email).OrderBy(e=>e.CreatedAt).ToListAsync();
                return events;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        //Update event//
        public async Task update_event(Event newEventData)
        {
            try
            {
                var idParam = new SqlParameter("@id", newEventData.Id);
                var titleParam = new SqlParameter("@title", newEventData.Title);
                var dateParam = new SqlParameter("@date", newEventData.Date);
                var startTimeParam = new SqlParameter("@start_time", newEventData.StartTime);
                var endTimeParam = new SqlParameter("@end_time", newEventData.EndTime);
                var locationParam = new SqlParameter("@location", newEventData.Location);
                var detailParam = new SqlParameter("@detail", newEventData.Detail);
                var eventTypeParam = new SqlParameter("@event_type", newEventData.EventType);
                var ticketCostParam = new SqlParameter("@ticket_cost", newEventData.TicketCost);
                var imgUrlParam = new SqlParameter("@img_url", newEventData.ImgUrl);
                var eventCategoriesParam = new SqlParameter("@event_categories", newEventData.EventCategories);
                var capacityParam = new SqlParameter("@capacity", newEventData.Capacity);

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_update_event @id, @title, @date, @start_time, @end_time, @location, @detail, @event_type, @ticket_cost, @img_url, @event_categories, @capacity",
                    idParam, titleParam, dateParam, startTimeParam, endTimeParam, locationParam, detailParam,
                    eventTypeParam, ticketCostParam, imgUrlParam, eventCategoriesParam, capacityParam
                );

            }
            catch(Exception ex )
            {
                Console.WriteLine( ex.ToString() );
                throw ex;
            }
        }

        //Delete Event//
        public async Task delete_event(int id)
        {
            try
            {
                var idParam = new SqlParameter("@id", id);

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_events_delete @id",
                    idParam
                );

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

    }
}
