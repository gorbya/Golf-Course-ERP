using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinksLabGolfSystem.Models {
    public class Sale {
        public DateTime TimeOfSale { get; set; }
        public double Price { get; set; }
        public int? CustomerId { get; set; }
    }
}
