using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IAttachmentService
    {
        IEnumerable<Attachment> GetAttachmentsByResponseId(int responseId);
        void AddAttachment(Attachment attachment);
        void DeleteAttachmentsByResponseId(int responseId);
    }
}