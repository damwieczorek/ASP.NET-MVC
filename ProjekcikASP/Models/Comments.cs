﻿using ProjekcikASP.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Comments
    {
        [Key]
        public int Id { get; set; }
        public int MovieId { get; set; }
        public string UserId { get; set; }
        public string Comment { get; set; }
        public virtual Movies Movie { get; set; }
        public virtual User User { get; set; }
    }
}