using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IResponseService
    {
        (bool, IEnumerable<Response>) GetResponse();
        int AddResponse(Response response);
        void DeleteResponsesByTicketId(int ticketId);
        void UpdateResponse(Response response);
        List<Response> GetResponsesByTicketId(int ticketId);
       
    }
}
