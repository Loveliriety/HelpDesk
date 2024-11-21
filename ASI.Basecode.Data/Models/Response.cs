using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Models
{
    public class Response
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ResponseId { get; set; }
        public int TicketId { get; set; }
        public string Sender { get; set; }
        public string Description { get; set; }
        public byte[] Attachment { get; set; }
        public DateTime CreatedTime { get; set; }

        //working
    }
}
