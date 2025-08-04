using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.UI.Xaml.Scheduler;

namespace LinksLabGolfSystem.Models.CourseReservations {
    public class CourseReservation {
        public int Uid { get; set; }
        public ScheduleAppointment Reservation { get; set; }
        
        //String may not be appropriate - revisit as necessary
        public int AreaReserved { get; set; } 
    }
}
