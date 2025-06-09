using AutoMapper;
using EventScheduler.Server.DTOs;
using EventScheduler.Server.Mappings;
using EventScheduler.Server.Models;
using EventScheduler.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventScheduler.Server.Controllers
{
    [ApiController]
    [Route("api/tickets")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticket_srv;
        private readonly IMapper _mapper;
        private readonly IEventService _event_srv;

        public TicketController(ITicketService ticket_srv, IMapper mapper, IEventService event_srv)
        {
            _ticket_srv = ticket_srv;
            _mapper = mapper;
            _event_srv = event_srv;
        }

        [HttpGet()]
        public async Task<ActionResult<List<Ticket>>> get_all_tickets()
        {
            try
            {
                List<Ticket>? tickets = await _ticket_srv.get_tickets();
                return Ok(tickets);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message.ToString());
                return StatusCode(500);
            }
        }

        //Endpoint handling creation of ticket for user//
        [Authorize]
        [HttpPost()]
        public async Task<ActionResult<List<TicketDto>>> ticket_creation([FromBody] TicketBuyDto ticketpayload)
        {
            try
            {
                var curr_user_email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
                Event? event_data = await _event_srv.get_event_with_id(ticketpayload.EventId);
                if (event_data == null)
                {
                    return BadRequest("Event not found");
                }
                int? event_capacity = event_data.Capacity;
                int event_ticket_count = await _ticket_srv.get_event_ticket_count(event_data.Id);
                if (event_capacity! < event_ticket_count)
                {
                    return BadRequest("Event capcity limit reached");
                }

                List<Ticket>? tickets = await _ticket_srv.create_ticket(ticketpayload.EventId, curr_user_email, ticketpayload.TicketCount);
                List<TicketDto> tickets_res = _mapper.Map<List<TicketDto>>(tickets);
                return Ok(tickets_res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message.ToString());
            }
        }

        //Enpoint for getting tickets of current user//
        [Authorize]
        [HttpGet("users")]
        public async Task<ActionResult<List<TicketDto>>> get_tickets()
        {
            try
            {
                var user_email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;

                List<Ticket>? tickets = await _ticket_srv.get_user_ticket_list(user_email!);
                List<TicketDto> tickets_res = _mapper.Map<List<TicketDto>>(tickets);
                return Ok(tickets_res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message.ToString());
            }
        }

        //Enpoint for updating ticket status//
        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}")]
        public async Task<ActionResult<TicketDto>> update_ticket_status(int id, [FromBody] TicketUpdateDto ticket_update)
        {
            try
            {
                Ticket? ticket = await _ticket_srv.get_ticket_with_id(id);
                if (ticket == null)
                {
                    return BadRequest("Ticket Not Found");
                }
                Event? event_data = await _event_srv.get_event_with_id(ticket.EventId);
                if (event_data == null)
                {
                    return BadRequest("Event not found");
                }
                var curr_user_email = HttpContext.User.FindFirst(ClaimTypes.Email).Value;
                if (curr_user_email == null || event_data.Organizer!=curr_user_email)
                {
                    throw new UnauthorizedAccessException("Unauthorized email");
                }

                Ticket? updated_ticket = await _ticket_srv.update_ticket(ticket_update.TicketId, ticket_update.Status);
                TicketDto? updated_ticket_dto = _mapper.Map<TicketDto>(updated_ticket);
                return Ok(updated_ticket_dto);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message.ToString());
            }
        }

        //Endpoint for getting invite ticket request//
        [Authorize(Roles ="Admin")]
        [HttpGet("invites")]
        public async Task<ActionResult<List<TicketDto>>> get_invite_tickets()
        {
            try
            {
                var organizer_email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;

                List<Ticket>? tickets = await _ticket_srv.get_organizer_ticket_list(organizer_email!);
                List<TicketDto> tickets_res = _mapper.Map<List<TicketDto>>(tickets);
                return Ok(tickets_res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message.ToString());
            }
        }

        [HttpGet("counts/{event_id}")]
        public async Task<ActionResult<int>> event_limit(int event_id)
        {
            try
            {
                int count = await _ticket_srv.get_event_ticket_count(event_id);
                return Ok(count);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

    }
}
