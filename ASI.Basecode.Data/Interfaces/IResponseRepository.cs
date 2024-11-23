using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Interfaces
{
    public interface IResponseRepository
    {
        Response GetResponseById(int responseId);
        IEnumerable<Response> GetAllResponses();
        void AddResponse(Response response);
        void UpdateResponse(Response response);
        void DeleteResponsesByTicketId(int ticketId);
        IEnumerable<Response> GetResponsesByTicketId(int ticketId);
        //working
    }
}
