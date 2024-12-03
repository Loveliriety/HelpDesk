using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Data.Repositories;
using ASI.Basecode.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASI.Basecode.Services.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IResponseService _responseService;

        public TicketService(ITicketRepository ticketRepository, IResponseService responseService)
        {
            _ticketRepository = ticketRepository;
            _responseService = responseService ?? throw new ArgumentNullException(nameof(responseService));
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

            int ticketId = _ticketRepository.AddTicket(newTicket);
            return ticketId;
        }

        public void DeleteTicket(int ticketId)
        {
            _responseService.DeleteResponsesByTicketId(ticketId);
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

        public IEnumerable<Ticket> GetTicketsByAssignee(string assignee)
        {
            return _ticketRepository.GetTicketsByAssignee(assignee);
        }

        public (bool, IEnumerable<TicketWithResponses>) GetTicketsWithResponses()
        {
            var tickets = _ticketRepository.GetAllTickets();
            if (tickets != null)
            {
                var ticketsWithResponses = tickets.Select(ticket => new TicketWithResponses
                {
                    Ticket = ticket,
                    Responses = _responseService.GetResponsesByTicketId(ticket.TicketId)
                }).ToList();

                return (true, ticketsWithResponses);
            }

            return (false, null);
        }

        //working
    }
}
