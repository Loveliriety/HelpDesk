using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Services
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IAttachmentRepository _attachmentRepository;

        public AttachmentService(IAttachmentRepository attachmentRepository)
        {
            _attachmentRepository = attachmentRepository;
        }

        public IEnumerable<Attachment> GetAttachmentsByResponseId(int responseId)
        {
            return _attachmentRepository.GetAttachmentsByResponseId(responseId);
        }

        public void AddAttachment(Attachment attachment)
        {
            _attachmentRepository.AddAttachment(attachment);
        }

        public void DeleteAttachmentsByResponseId(int responseId)
        {
            if (responseId <= 0)
            {
                throw new ArgumentException("Invalid response Id.", nameof(responseId));
            }

            var attachments = _attachmentRepository.GetAttachmentsByResponseId(responseId);

            if (attachments == null || !attachments.Any())
            {
                Console.WriteLine($"No attachments found for Response ID: {responseId}");
                return;
            }

            try
            {
                foreach (var attachment in attachments)
                {
                    _attachmentRepository.DeleteAttachment(attachment.AttachmentId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting attachments: {ex.Message}");
                throw;
            }
        }

    }
}