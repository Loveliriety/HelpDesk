using System;

namespace ASI.Basecode.WebApp.Models
{
    public class ResponseViewModel
    {
        public int ResponseId { get; set; }
        public int TicketId { get; set; }
        public string Sender { get; set; }
        public string Description { get; set; }
        public byte[] Attachment { get; set; }
        public DateTime CreatedTime { get; set; }

        //working
    }
}
