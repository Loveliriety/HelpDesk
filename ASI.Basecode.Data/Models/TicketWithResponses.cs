using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Models
{
    public class TicketWithResponses
    {
        public Ticket Ticket { get; set; }
        public IEnumerable<Response> Responses { get; set; }
    }
}
