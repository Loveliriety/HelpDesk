using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Migrations;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Repositories
{
    public class AttachmentRepository : BaseRepository, IAttachmentRepository
    {
        private readonly AsiBasecodeDBContext _dbContext;

        public AttachmentRepository(AsiBasecodeDBContext dBContext, UnitOfWork unitOfWork) : base(unitOfWork)
        { 
            _dbContext = dBContext;
        }

        public IEnumerable<Attachment> GetAttachmentsByResponseId(int responseId)
        {
            return _dbContext.Attachments
                 .Where(attachment => attachment.ResponseId == responseId)
                 .ToList();
        }
        public void AddAttachment(Attachment attachment)
        {
            _dbContext.Set<Attachment>().Add(attachment);
            _dbContext.SaveChanges();
        }
        public void DeleteAttachment(int attachmentId)
        {
            if (attachmentId <= 0)
            {
                throw new ArgumentException("Invalid attachment ID.", nameof(attachmentId));
            }

            try
            {
                var attachment = _dbContext.Set<Attachment>().Find(attachmentId);

                if (attachment == null)
                {
                    Console.WriteLine($"Attachment with ID {attachmentId} not found.");
                    return; 
                }

                _dbContext.Set<Attachment>().Remove(attachment);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting attachment: {ex.Message}");
                throw;
            }
        }
    }
}
