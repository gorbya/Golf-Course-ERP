using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinksLabGolfSystem.Models {
    public class Equipment {
        public int Uid { get; set; }
        public string Description { get; set; }
        public string MaintenanceFrequency { get; set; }
        public DateTime LastDateMaintained {get; set; }
        public bool IsAvailable { get; set; }
    }
}
