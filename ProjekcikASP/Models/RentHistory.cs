using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjekcikASP.Models
{
    public class RentHistory
    {
        [Key]
        public int Id { get; set; }
        public int MovieId { get; set; }
        public string UserId { get; set; }
        public DateTime RentDate { get; set; }
        public int RentTime { get; set; }
       public virtual Movies Movie { get; set; }
        public virtual User User { get; set; }


    }
}