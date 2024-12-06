using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class TicketPageViewModel
    {
        // Ticket
        public int TicketId { get; set; }
        public string Status { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public int Category { get; set; }
        public string RequesterEmail { get; set; }
        public string Assignee { get; set; }
        public string Priority { get; set; }
        public bool IsFeedbackOffered { get; set; }
        public bool? Feedback { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }

        // Category
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        // Team
        public int TeamId { get; set; }
        public string TeamName { get; set; }

        public int SelectedCategoryId { get; set; }
        public string SelectedCategoryName { get; set; }

        public List<ResponseViewModel> Responses { get; set; }


        //working
    }

}
