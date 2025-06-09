using EventScheduler.Server.DTOs;
using EventScheduler.Server.Models;
using EventScheduler.Server.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EventScheduler.Server.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _event_repository;
        public EventService(IEventRepository event_repository)
        {
            _event_repository = event_repository;
        }

        //Create Event//
        public async Task<Event> create_event(Event new_event)
        {
            try
            {

                Event created_event = await _event_repository.add_event(new_event);
                return created_event;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get All Events//
        public async Task<List<Event>?> get_all_events()
        {
            try
            {
                List<Event>? events = await _event_repository.get_all_events();
                return events;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        //Get Event with Id//
        public async Task<Event?> get_event_with_id(int eventId)
        {
            try
            {
                Event? eventData = await _event_repository.get_event(eventId);
                return eventData;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        //Get all current user event//
        public async Task<List<Event>?> get_my_events(string user_email)
        {
            try
            {
                List<Event>? my_events = await _event_repository.user_events(user_email);
                return my_events;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        //Update Event//
        public async Task update_event(Event new_event_data)
        {
            try
            {
                await _event_repository.update_event(new_event_data);
                return;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }
        
        //Delete Event with Id//
        public async Task<string> delete_event(int id)
        {
            try
            {
                await _event_repository.delete_event(id);
                return "Event Deleted";
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
        }

    }
}
