using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinksLabGolfSystem.Models {
    public class Equipment {
        public int Uid { get; set; }
        public string Description { get; set; }
        public TimeSpan MaintenanceFrequency { get; set; }
    }
}
