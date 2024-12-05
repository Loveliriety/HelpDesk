using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Data.Repositories;
using ASI.Basecode.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Services
{
    public class ResponseService : IResponseService
    {
        private readonly IResponseRepository _responseRepository;

        public ResponseService(IResponseRepository responseRepository)
        {
            _responseRepository = responseRepository;
        }

        public (bool, IEnumerable<Response>) GetAllResponses()
        {
            var responses = _responseRepository.GetAllResponses();
            if (responses != null && responses.Any())
            {
                return (true, responses);
            }
            else
            {
                return (false, null);
            }
        }

        public (bool, IEnumerable<Response>) GetResponse()
        {
            var responses = _responseRepository.GetAllResponses();

            if (responses != null && responses.Any())
            {
                return (true, responses);  
            }
            else
            {
                return (false, Enumerable.Empty<Response>()); 
            }
        }
        public int AddResponse(Response response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            var newResponse = new Response
            {
                TicketId = response.TicketId,
                Sender = response.Sender,
                Description = response.Description,
                CreatedTime = response.CreatedTime
            };

            int responseId = _responseRepository.AddResponse(newResponse);
            return responseId; 
        }

        public void DeleteResponsesByTicketId(int ticketId)
        {
            if (ticketId <= 0)
            {
                throw new ArgumentException("Invalid ticket ID.", nameof(ticketId));
            }

            var responses = _responseRepository.GetResponsesByTicketId(ticketId);

            if (responses == null || !responses.Any())
            {
                Console.WriteLine($"No responses found for Ticket ID: {ticketId}");
                return;
            }

            try
            {
                foreach (var response in responses)
                {
                    _responseRepository.DeleteResponse(response.ResponseId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting responses: {ex.Message}");
                throw;
            }
        }

        public void UpdateResponse(Response response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            _responseRepository.UpdateResponse(response);
        }

        public List<Response> GetResponsesByTicketId(int ticketId)
        {
            if (ticketId <= 0)
            {
                throw new ArgumentException("Invalid ticket ID", nameof(ticketId));
            }

            var responses = _responseRepository.GetResponsesByTicketId(ticketId).ToList();

            return responses ?? new List<Response>();
        }

        //working
    }
}
