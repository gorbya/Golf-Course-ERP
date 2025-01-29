using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinksLabGolfSystem.Models.EquipmentReservations {
    public class EquipmentRental : EquipmentReservation {
        public int CustomerId { get; set; }
        public Sale SaleInformation { get; set; }
    }
}
