using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TicketingAppCloud.DBModels;
using TicketingAppCloud.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TicketingAppCloud.Controllers
{
    
    [Route("[controller]")]
    public class TicketController : Controller
    {
        private readonly TicketingDbContext _ticketingDbContext;

        public TicketController(TicketingDbContext ticketingDbContext)
        {
            _ticketingDbContext = ticketingDbContext;
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var tickets = _ticketingDbContext.Tickets.ToList().OrderByDescending(x => x.UpdatedDate != null ? x.UpdatedDate.Value : x.CreatedDate).ToList();
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(ex.ToString()));
            }
        }
        
        [Authorize]
        [HttpGet]
        [Route("mytickets")]
        public IActionResult Get(string userName)
        {
            try
            {
                var loggedinUser = this.User.FindFirst(ClaimTypes.Name.ToString())?.Value;
                if (!string.IsNullOrEmpty(loggedinUser))
                {
                    var tickets = _ticketingDbContext.Tickets.Where(x => x.AssignedTo == loggedinUser).ToList().OrderByDescending(x => x.UpdatedDate != null ? x.UpdatedDate.Value : x.CreatedDate).ToList();
                    return Ok(tickets);
                }
                else
                {
                    return Unauthorized();
                }
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(ex.ToString()));
            }
        }
        
        [HttpGet("Dashboard")]
        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var ticket = _ticketingDbContext.Tickets.FirstOrDefault(x => x.Id == id);
                if (ticket != null)
                {
                    return Ok(ticket);
                }
                else
                {
                    return NotFound(new ApiResponse($"Ticket with ID {id} not found."));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(ex.ToString()));
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]Ticket ticket)
        {
            try
            {
                var t = _ticketingDbContext.Tickets.Add(ticket);
                _ticketingDbContext.SaveChanges();
                return Ok(new ApiResponse("Ticket added successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(ex.ToString()));
            }
        }

        [HttpPut("Update")]
        public IActionResult Put([FromBody]Ticket ticket)
        {
            try
            {
                var isTicketExist = _ticketingDbContext.Tickets.Any(x => x.Id == ticket.Id);
                if (isTicketExist)
                {
                    ticket.UpdatedDate = DateTime.UtcNow;
                    var ticketResp = _ticketingDbContext.Tickets.Update(ticket);
                    _ticketingDbContext.SaveChanges();
                    return Ok(new ApiResponse("Ticket updated successfully."));
                }
                else
                {
                    return NotFound(new ApiResponse($"Ticket with ID {ticket.Id} not found."));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(ex.ToString()));
            }
        }

        [HttpPut("Sync")]
        public IActionResult Sync([FromBody] List<Ticket> tickets)
        {
            try
            {
                var apiResponses = new List<ApiResponse>();
                foreach (var ticket in tickets)
                {
                    var isTicketExist = _ticketingDbContext.Tickets.Any(x => x.Id == ticket.Id);
                    if (isTicketExist)
                    {
                        ticket.UpdatedDate = DateTime.UtcNow;
                        var ticketResp = _ticketingDbContext.Tickets.Update(ticket);
                        _ticketingDbContext.SaveChanges();
                        apiResponses.Add(new ApiResponse($"Ticket with ID {ticket.Id} updated successfully."));
                    }
                    else
                    {
                        apiResponses.Add(new ApiResponse($"Ticket with ID {ticket.Id} not found."));
                    }
                }

                return Ok(apiResponses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(ex.ToString()));
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var ticket = _ticketingDbContext.Tickets.FirstOrDefault(x => x.Id == id);
                if (ticket != null)
                {
                    var ticketResp = _ticketingDbContext.Tickets.Remove(ticket);
                    _ticketingDbContext.SaveChanges();
                    return Ok(new ApiResponse("Ticket deleted successfully."));
                }
                else
                {
                    return NotFound(new ApiResponse($"Ticket with ID {id} not found."));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(ex.ToString()));
            }
        }
    }
}
