﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinksLabGolfSystem.Models {
    public class Customer {
        public int Uid { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsMember { get; set; }
        public DateTime RenewalDate { get; set; }
    }
}
