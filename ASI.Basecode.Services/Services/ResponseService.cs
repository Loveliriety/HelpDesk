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

            _responseRepository.AddResponse(response);
            return response.ResponseId; 
        }

        public void DeleteResponse(int responseId)
        {
            if (responseId <= 0)
            {
                throw new ArgumentException("Invalid response ID", nameof(responseId));
            }

            _responseRepository.DeleteResponse(responseId);
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

            return _responseRepository.GetAllResponses()
                                       .Where(r => r.TicketId == ticketId)
                                       .ToList();
        }

        //working
    }
}
