using ASI.Basecode.Data.Models;
using ASI.Basecode.Data.Repositories;
using ASI.Basecode.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace ASI.Basecode.Services.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;

        public TicketService(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public (bool, IEnumerable<Ticket>) GetAllTickets()
        {
            var tickets = _ticketRepository.GetAllTickets();
            if (tickets != null)
            {
                return (true, tickets);
            }
            else
            {
                return (false, null);
            }
        }

        public int AddTicket(Ticket ticket)
        {
            if (ticket == null)
            {
                throw new ArgumentNullException(nameof(ticket));
            }

            var newTicket = new Ticket
            {
                Category = ticket.Category,
                Subject = ticket.Subject,
                Status = ticket.Status,
                Priority = ticket.Priority,
                RequesterEmail = ticket.RequesterEmail,
                Assignee = ticket.Assignee,
                CreatedTime = DateTime.Now,
                UpdatedTime = DateTime.Now
            };

            return _ticketRepository.AddTicket(newTicket);
        }

        public void DeleteTicket(int ticketId)
        {
            _ticketRepository.DeleteTicket(ticketId);
        }

        public void UpdateTicket(Ticket ticket)
        {
            if (ticket == null)
            {
                throw new ArgumentNullException(nameof(ticket));
            }
            _ticketRepository.UpdateTicket(ticket);
        }

        public Ticket GetTicketById(int ticketId)
        {
            return _ticketRepository.GetTicketById(ticketId);
        }

        public IEnumerable<Ticket> GetTicketsByCategory(int category)
        {
            return _ticketRepository.GetTicketsByCategory(category);
        }

        public IEnumerable<Ticket> GetTicketsByStatus(string status)
        {
            return _ticketRepository.GetTicketsByStatus(status);
        }

        public IEnumerable<Ticket> GetTicketsByPriority(string priority)
        {
            return _ticketRepository.GetTicketsByPriority(priority);
        }

        //working
    }
}
