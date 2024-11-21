using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Repositories
{
    public interface ITicketRepository
    {
        Ticket GetTicketById(int ticketId);
        IEnumerable<Ticket> GetAllTickets();
        int AddTicket(Ticket ticket);
        void UpdateTicket(Ticket ticket);
        void DeleteTicket(int ticketId);
        List<Ticket> GetTicketsByCategory(int category);
        List<Ticket> GetTicketsByStatus(string status);
        List<Ticket> GetTicketsByPriority(string priority);
    }
}
