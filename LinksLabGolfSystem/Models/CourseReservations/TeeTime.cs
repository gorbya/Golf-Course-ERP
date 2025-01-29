using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinksLabGolfSystem.Models.CourseReservations {
    public class TeeTime : CourseReservation {
        public int? CustomerUid { get; set; }
        public int NumberOfGolfers { get; set; }
        public Sale SaleInformation { get; set; }
    }
}
