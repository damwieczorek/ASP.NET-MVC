using ProjekcikASP.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Categories
    {
        [Key]
        public int Id { get; set; }
        public string Category { get; set; }
        public virtual ICollection<Movies> Movie { get; set; }
    }
}