﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGHelper.Models
{
    public class User
    {
        public User()
        {
            this.Heroes = new List<Hero>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public virtual ICollection<Hero> Heroes { get; set; }
    }
}
