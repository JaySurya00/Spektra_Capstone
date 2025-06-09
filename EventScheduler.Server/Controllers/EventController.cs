using AutoMapper;
using EventScheduler.Server.DTOs;
using EventScheduler.Server.Models;
using EventScheduler.Server.Repositories;
using EventScheduler.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace EventScheduler.Server.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventController: ControllerBase
    {
        private readonly IEventService _event_srv;
        private readonly IMapper _mapper;
        public EventController(IEventService event_srv, IMapper mapper) {
            _event_srv = event_srv;
            _mapper = mapper;
        }

        //Add Event Endpoint//
        [Authorize(Roles ="Admin")]
        [HttpPost()]
        public async Task<ActionResult<EventDto>> CreateEvent([FromBody] EventDto eventData)
        {
            try
            {
                var claim_name = HttpContext.User.FindFirst(ClaimTypes.Name);
                var claim_email = HttpContext.User.FindFirst(ClaimTypes.Email);
                var claim_role = HttpContext.User.FindFirst(ClaimTypes.Role);
                Random random = new Random();
                var event_id = random.Next();
                if (claim_name == null)
                {
                    return BadRequest(new ErrorDto { error_msgs = ["Please provide email"] });
                }

                Event new_event = new Event()
                {
                    Id= event_id,
                    Title = eventData.Title,
                    Date= eventData.Date,
                    StartTime= eventData.StartTime,
                    EndTime= eventData.EndTime,
                    Location= eventData.Location,
                    Capacity= eventData.Capacity,
                    Detail= eventData.Detail,
                    ImgUrl= eventData.ImgUrl,
                    EventCategories= eventData.EventCategories,
                    EventType= eventData.EventType,
                    TicketCost= eventData.TicketCost,
                    Organizer= claim_email.Value,
                    CreatedAt= null
                };
                Event created_event = await _event_srv.create_event(new_event);
                EventDto event_res = _mapper.Map<EventDto>(created_event);
                return StatusCode(201, event_res);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message.ToString());
            }
        }


        //Get All Events Endpoint//
        [HttpGet()]
        public async Task<ActionResult<List<EventDto>>> get_all_events()
        {
            try
            {
                List<Event>? events = await _event_srv.get_all_events();
                List<EventDto>? events_res = _mapper.Map<List<EventDto>>(events);
                return Ok(events_res);
            }
            catch(Exception ex)
            {
                return null;
            }
        }


        //Get Event with Id endpoint//
        [HttpGet("{eventId}")]
        public async Task<ActionResult<EventDto>> get_event(int eventId)
        {
            try
            {
                Event? eventData = await _event_srv.get_event_with_id(eventId);
                if (eventData == null)
                {
                    return BadRequest(new ErrorDto { error_msgs = ["No Event Found"] });
                }
                EventDto? event_res = _mapper.Map<EventDto>(eventData);
                return Ok(event_res);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //Get current admin events endpoint//
        [Authorize(Roles ="Admin")]
        [HttpGet("myevents")]
        public async Task<ActionResult<EventDto>> get_my_events()
        {
            try
            {
                string? user_email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
                if (user_email == null)
                {
                    return Unauthorized();
                }
                List<Event>? eventData = await _event_srv.get_my_events(user_email);
                List<EventDto>? event_res = _mapper.Map<List<EventDto>>(eventData);
                return Ok(event_res);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return StatusCode(500, ex.Message.ToString());
            }
        }

        //Delete event endpoint//
        [Authorize(Roles ="Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> delete_event(int id)
        {
            try
            {
                await _event_srv.delete_event(id);
                return NoContent();
            }
            catch(Exception ex)
            {
                return Ok(ex.Message.ToString());
            }
        }
        //Update Event Endpoint//
        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> update_event(int id, [FromBody] EventDto newEventData)
        {
            try
            {
                Event newEvent = _mapper.Map<Event>(newEventData);
                await _event_srv.update_event(newEvent);
                return NoContent();
            }
            catch (Exception ex)
            {
                return Ok(ex.Message.ToString());
            }
        }
    }
}
