﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChadCalendar.Models
{
    public class Duty
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime Accessed { get; set; }
        public int NRepetitions { get; set; }
        public int Multiplier { get; set; }
        public string Frequency { get; set; }
        [Required]
        public User User { get; set; }
    }
}
