using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class AttachmentViewModel
    {
        public int AttachmentId { get; set; }
        public int ResponseId { get; set; }
        public string FileData { get; set; }
    }
}