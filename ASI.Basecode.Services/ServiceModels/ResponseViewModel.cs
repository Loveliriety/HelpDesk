using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class ResponseViewModel
    {
        public int ResponseId { get; set; }
        public int TicketId { get; set; }
        public string Sender { get; set; }
        public string SenderId { get; set; }
        public string Description { get; set; }
        public byte[] Attachment { get; set; }
        public DateTime CreatedTime { get; set; }

        public List<AttachmentViewModel> Attachments { get; set; }
        //working
    }
}
