using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinksLabGolfSystem.Models.CourseReservations {
    public class CourseReservation {
        public int Uid { get; set; }
        public DateTime ReservationTime { get; set; }
        public DateTime EstimatedCompletionTime { get; set; }
        
        //String may not be appropriate - revisit as necessary
        public string AreaReserved { get; set; } 
    }
}
