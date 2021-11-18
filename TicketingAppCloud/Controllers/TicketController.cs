using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
                var tickets = _ticketingDbContext.Tickets.ToList();
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(ex.ToString()));
            }
            //return new List<object>
            //{
            //    new {Id = 1, Summary = "Printer SX001876 is not working", Details = "There is ink leakage from the printer", Priority = "High", Status = "Open", AssignedTo = "John Doe"},
            //    new {Id = 2, Summary = "Printer PY009872 is not working properly", Details = "There is print quality is Bad", Priority = "Medium", Status = "In Progress", AssignedTo = "Mike Pant"},
            //    new {Id = 3, Summary = "Uncertain about printer 9CAAQ01 security", Details = "", Priority = "Low", Status = "Resolved", AssignedTo = "Don Bravo"},
            //};
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

        [HttpPut]
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
