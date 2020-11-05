using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace ProjekcikASP.Models
{
    public class Movies
    {
        [Key]
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string UserId { get; set; }
        public string MovieName { get; set; }        
        public string MovieDescription { get; set; }        
        public string MovieDirector { get; set; }
        public float MovieRentPrice { get; set; }
        public int MovieRealiseDate { get; set; }
        public string MovieOriginCountry { get; set; }
        public string MoviePhoto { get; set; }
        public string MovieTrailer { get; set; }

        public virtual User User { get; set; }
        public virtual Categories Category { get; set; }
        public virtual ICollection<Rating> Rating { get; set; }
        public virtual ICollection<Comments> Comments { get; set; }
    }
}