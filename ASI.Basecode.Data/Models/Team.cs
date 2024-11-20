using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Models
{
    public class Team
    {
        [Key]
        public int TeamId { get; set; }

        [Required(ErrorMessage = "Team Name is required!")]
        public string TeamName {  get; set; }

        [Required(ErrorMessage = "Company is required!")]
        public string Company {  get; set; }

        [Required(ErrorMessage = "Tier is required!")]
        public string Tier { get; set; }

        [Required(ErrorMessage = "Manager is required!")]
        public string Manager { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
