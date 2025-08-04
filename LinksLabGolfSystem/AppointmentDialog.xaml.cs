using Syncfusion.UI.Xaml.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json;
using SqlHelper;

namespace LinksLabGolfSystem {
    /// <summary>
    /// Interaction logic for AppointmentDialog.xaml
    /// </summary>
    public partial class AppointmentDialog : Window {
        private ScheduleAppointment _Appointment;

        public event EventHandler<ScheduleAppointment> ProceedToCheckout;

        public AppointmentDialog(ScheduleAppointment appointment) {
            InitializeComponent();
            _Appointment = appointment;

            SubjectText.Text = $"{_Appointment.Subject}";
            StartTimeText.Text = $"Tee Time: {_Appointment.StartTime}";
            DescriptionText.Text = $"{_Appointment.Notes}";
        }

        private void CheckoutButton_Click(object sender, RoutedEventArgs e) {
            ProceedToCheckout?.Invoke(this, _Appointment); // Raise event
            this.Close(); // Close dialog
        }
    }
}