using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.ServiceModels;
using System.Collections.Generic;

namespace ASI.Basecode.Services.Interfaces
{
    public interface ITicketService
    {
        (bool, IEnumerable<Ticket>) GetAllTickets();
        int AddTicket(Ticket ticket);
        void DeleteTicket(int ticketId);
        void UpdateTicket(Ticket ticket);
        Ticket GetTicketById(int ticketId);
        IEnumerable<Ticket> GetTicketsByCategory(int category);
        IEnumerable<Ticket> GetTicketsByStatus(string status);
        IEnumerable<Ticket> GetTicketsByPriority(string priority);
        IEnumerable<Ticket> GetTicketsByAssignee(string assignee);
        (bool, IEnumerable<TicketWithResponses>) GetTicketsWithResponses();
    }
}
