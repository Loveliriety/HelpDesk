﻿using ASI.Basecode.Data.Models;
using System.Collections.Generic;
using ASI.Basecode.Services.ServiceModels;

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
        IEnumerable<Ticket> GetTicketsByAssignee(int assignee);
        (bool, IEnumerable<TicketWithResponses>) GetTicketsWithResponses();
    }
}
