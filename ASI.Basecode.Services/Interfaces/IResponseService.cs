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
        public (bool, IEnumerable<Response>) GetResponse();
        public int AddResponse(Response response);
        public void DeleteResponse(int response);
        public void UpdateResponse(Response response);
        List<Response> GetResponsesByTicketId(int ticketId);

        //working
    }
}
