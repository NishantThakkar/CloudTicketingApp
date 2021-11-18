using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketingAppCloud.DBModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TicketingAppCloud.Controllers
{
    [Route("[controller]")]
    //[ApiController]
    public class TicketController : Controller
    {
        private readonly TicketingDbContext _ticketingDbContext;
        public TicketController(TicketingDbContext ticketingDbContext)
        {
            _ticketingDbContext = ticketingDbContext;
        }
        // GET: api/<TicketController>
        [HttpGet]
        public IEnumerable<Ticket> Get()
        {
            var tickets = _ticketingDbContext.Tickets.ToList();
            return tickets;
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

        // GET api/<TicketController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TicketController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TicketController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TicketController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
