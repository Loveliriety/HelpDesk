using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Interfaces
{
    public interface IAttachmentRepository
    {
        IEnumerable<Attachment> GetAttachmentsByResponseId(int responseId);
        void AddAttachment(Attachment attachment);
        void DeleteAttachment(int responseId);
    }
}