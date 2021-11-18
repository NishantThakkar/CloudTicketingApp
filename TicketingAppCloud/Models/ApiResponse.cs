using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketingAppCloud.Models
{
    public class ApiResponse
    {
        public ApiResponse(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}
