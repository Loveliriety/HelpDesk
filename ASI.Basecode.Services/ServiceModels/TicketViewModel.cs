using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class TicketViewModel
    {
        public int TicketId { get; set; }
        public string Status { get; set; }
        public string Subject { get; set; }
        public int Category { get; set; }
        public string RequesterEmail { get; set; }
        public string Assignee { get; set; }
        public string Priority { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
