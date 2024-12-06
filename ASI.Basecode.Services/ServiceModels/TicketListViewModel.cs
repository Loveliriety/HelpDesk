using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class TicketListViewModel
    {
        public List<TicketPageViewModel> Tickets { get; set; }
        public TicketPageViewModel SelectedTicket { get; set; }
        public List<CategoryViewModel> Categories { get; set; }
        public List<UserViewModel> Users { get; set; }
        public List<ResponseViewModel> Responses { get; set; }
        public List<TeamViewModel> Teams { get; set; }

        public int CurrentPage { get; set; } 
        public int TotalPages { get; set; }

        //working
    }
}
