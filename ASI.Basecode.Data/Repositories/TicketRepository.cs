using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Repositories
{
    public class TicketRepository : BaseRepository, ITicketRepository
    {
        private readonly AsiBasecodeDBContext _dbContext;

        public TicketRepository(AsiBasecodeDBContext dbContext, UnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = dbContext;
        }

        public Ticket GetTicketById(int ticketId)
        {
            return _dbContext.Set<Ticket>().Find(ticketId);
        }

        public IEnumerable<Ticket> GetAllTickets()
        {
            return _dbContext.Tickets.ToList();
        }

        public int AddTicket(Ticket ticket)
        {
            _dbContext.Set<Ticket>().Add(ticket);
            _dbContext.SaveChanges();
            return ticket.TicketId;
        }

        public void UpdateTicket(Ticket ticket)
        {
            _dbContext.Set<Ticket>().Update(ticket);
            _dbContext.SaveChanges();
        }

        public void DeleteTicket(int ticketId)
        {
            var ticket = _dbContext.Set<Ticket>().Find(ticketId);
            if (ticket != null)
            {
                _dbContext.Set<Ticket>().Remove(ticket);
                _dbContext.SaveChanges();
            }
        }

        public List<Ticket> GetTicketsByCategory(int category)
        {
            return _dbContext.Set<Ticket>().Where(t => t.Category == category).ToList();
        }

        public List<Ticket> GetTicketsByStatus(string status)
        {
            return _dbContext.Set<Ticket>().Where(t => t.Status == status).ToList();
        }
        public List<Ticket> GetTicketsByPriority(string priority)
        {
            return _dbContext.Set<Ticket>().Where(t => t.Priority == priority).ToList();
        }

        //working
    }
}
