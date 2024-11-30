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
        private readonly ResponseRepository _responseRepository;
        private readonly ResponseService _responseService;

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
                Description = ticket.Description,
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

        //public bool DeleteTicketWithResponses(int ticketId)
        //{
        //    try
        //    {
        //        var responses = _responseService != null ? _responseService.GetResponsesByTicketId(ticketId) : new List<Response>();

        //        if (responses.Any())
        //        {
        //            foreach (var response in responses)
        //            {
        //                _responseRepository.DeleteResponse(response.ResponseId);
        //            }
        //        }

        //        _ticketRepository.DeleteTicket(ticketId);

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error deleting ticket and responses: {ex.Message}");
        //        return false;
        //    }
        //}

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

        //working
    }
}
