﻿using ASI.Basecode.Data.Models;
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
        int AddResponse(Response response);
        void UpdateResponse(Response response);
        void DeleteResponse(int responseId);
        IEnumerable<Response> GetResponsesByTicketId(int ticketId);
        //working
    }
}
