using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinksLabGolfSystem.Models.EquipmentReservations {
    public class EquipmentReservation {
        public int EquipmentUid { get; set; }
        public DateTime CheckoutTime { get; set; }
        public DateTime EstimatedReturnTime { get; set; }
    }
}
