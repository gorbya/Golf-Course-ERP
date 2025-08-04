using Syncfusion.UI.Xaml.Scheduler;
using Syncfusion.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using LinksLabGolfSystem.Models.CourseReservations;
using Microsoft.Xaml.Behaviors.Core;
using Newtonsoft.Json;
using SqlHelper;
using LinksLabGolfSystem.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Data.SqlClient;

namespace LinksLabGolfSystem {
    /// <summary>
    /// Interaction logic for Scheduler.xaml
    /// </summary>
    public partial class Scheduler : UserControl {

        public static event EventHandler<ScheduleAppointment> CheckoutRequested;

        private ObservableCollection<Customer> _Customers;
        
        public Customer SelectedCustomer { get; set; }

        public ObservableCollection<string> Courses {
            get {return new ObservableCollection<string>() {
                    "East", "West", "South"
                };
            }
        }
        public ObservableCollection<string> Holes {
            get {return new ObservableCollection<string>() {
                    "9", "18"
                };
            }
        }
        public ObservableCollection<Customer> Customers {
            get {
                if (_Customers == null) {
                    _Customers = new ObservableCollection<Customer>();
                }

                return _Customers;
            }
            set {
                _Customers = value;
                OnPropertyChanged(nameof(Customers));
            }
        }


        public Scheduler() {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(DataConstants.Keys.SyncFusionKey);
            InitializeComponent();
            DataContext = this;
            LoadAppointments();
            GetCustomers();
        }

        public void GetCustomers() {

            Customers.Clear();

            SQLDataService sdq = new SQLDataService(DataConstants.Keys.ConnectionString);

            DataTable dt = sdq.ExecuteSelectQuery("Select * from Customers where IsDeleted = 0");
            ObservableCollection<Customer> customerList = new ObservableCollection<Customer>();
            foreach (DataRow row in dt.Rows) {
                Customer customer = new Customer {
                    Uid = Convert.ToInt32(row["Uid"]),
                    FirstName = row["FirstName"].ToString(),
                    LastName = row["LastName"].ToString(),
                    Email = row["Email"].ToString(),
                    IsMember = (bool)row["IsMember"],
                    RenewalDate = row["RenewalDate"].ToString()
                };
                customerList.Add(customer);
            }

            Customers = customerList;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private void LoadAppointments() {

            SQLDataService sdq = new SQLDataService(DataConstants.Keys.ConnectionString);

            DataTable dt = sdq.ExecuteSelectQuery("Select * from CourseReservations where ReservationArea = 1");
            
            var eastAppointments = new ObservableCollection<ScheduleAppointment>();

            foreach (DataRow row in dt.Rows) {
                string apptInfo = row["ScheduleAppointment"].ToString();
                ScheduleAppointment ap = JsonConvert.DeserializeObject<ScheduleAppointment>(apptInfo);

                eastAppointments.Add(ap);

            }

            eastScheduler.ItemsSource = eastAppointments;


            dt = sdq.ExecuteSelectQuery("Select * from CourseReservations where ReservationArea = 2");

            var southAppointments = new ObservableCollection<ScheduleAppointment>();

            foreach (DataRow row in dt.Rows) {
                string apptInfo = row["ScheduleAppointment"].ToString();
                ScheduleAppointment ap = JsonConvert.DeserializeObject<ScheduleAppointment>(apptInfo);

                southAppointments.Add(ap);

            }

            southScheduler.ItemsSource = southAppointments;


            dt = sdq.ExecuteSelectQuery("Select * from CourseReservations where ReservationArea = 3");

            var westAppointments = new ObservableCollection<ScheduleAppointment>();

            foreach (DataRow row in dt.Rows) {
                string apptInfo = row["ScheduleAppointment"].ToString();
                ScheduleAppointment ap = JsonConvert.DeserializeObject<ScheduleAppointment>(apptInfo);

                westAppointments.Add(ap);

            }

            westScheduler.ItemsSource = westAppointments;

        }



        private void ButtonBase_OnClick(object sender, RoutedEventArgs e) {

            //Set background colors 

            ScheduleAppointment ap = new ScheduleAppointment();

            DateTime dt = (DateTime)DatePicker.Value;

            // Extract the time part correctly
            TimeSpan time = ((DateTime)TimePicker.Value).TimeOfDay;

            // Combine date and time
            DateTime combinedDateTime = dt.Date + time;
            ap.StartTime = combinedDateTime;
            ap.Subject = $"{SelectedCustomer.FullName} {txtHoles.Text} Hole Tee Time";
            ap.EndTime = combinedDateTime + TimeSpan.FromMinutes(5);
            ap.Notes = $"Number of golfers: {txtNumOfGolfers.Text}";

            CourseReservation cr = new CourseReservation();

            if (txtCourse.Text == "East") {
                cr.AreaReserved = 1;
            }
            else if (txtCourse.Text == "South")
            {
                cr.AreaReserved = 2;
            }
            else if (txtCourse.Text == "West")
            {
                cr.AreaReserved = 3;
            }

            cr.Reservation = ap;  
            SQLDataService sq = new SQLDataService(DataConstants.Keys.ConnectionString);

            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@ReservationArea", cr.AreaReserved));
            parameters.Add(new SqlParameter("@ScheduleAppointment", JsonConvert.SerializeObject(cr.Reservation)));
            parameters.Add(new SqlParameter("@CustomerUid", SelectedCustomer.Uid));

            string query = @"INSERT INTO CourseReservations (ReservationArea, ScheduleAppointment, CustomerUid) 
                 VALUES (@ReservationArea, @ScheduleAppointment, @CustomerUid);";


            sq.ExecuteNonQueryWithParams(query, parameters);

            LoadAppointments();

            
        }

        private void UpdateTimelineSlotWidth() {
            if (southScheduler.TimelineViewSettings is TimelineViewSettings settings) {
                double schedulerWidth = southScheduler.ActualWidth;

                int startHour = 7;
                int endHour = 20;
                int totalSlots = endHour - startHour;

                double padding = 0;
                double availableWidth = schedulerWidth - padding;

                if (availableWidth > 0 && totalSlots > 0){
                    double tis = availableWidth / totalSlots;
                    settings.TimeIntervalSize = tis;
                    westScheduler.TimelineViewSettings.TimeIntervalSize = tis;
                    eastScheduler.TimelineViewSettings.TimeIntervalSize = tis;
                }
            }
        }

        private void southScheduler_Loaded(object sender, RoutedEventArgs e){
            UpdateTimelineSlotWidth();
        }

        private void scheduler_SizeChanged(object sender, SizeChangedEventArgs e) {
            UpdateTimelineSlotWidth();
        }

        private void Scheduler_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            if (e.OriginalSource is FrameworkElement element && element.DataContext is ScheduleAppointment appointment) {
                var dialog = new AppointmentDialog(appointment);
                dialog.ProceedToCheckout += (s, appt) =>
                {
                    // Raise event to notify parent (MainWindow)
                    CheckoutRequested?.Invoke(this, appt);
                };
                dialog.ShowDialog();
            }
        }
        private void EventButtonBase_OnClick(object sender, RoutedEventArgs e) {
            var timePickerWindow = new TimePickerWindow();
            if (timePickerWindow.ShowDialog() == true)
            {
                DateTime selectedTime = timePickerWindow.SelectedTime;

                var t = 0;

                ScheduleAppointment ap = new ScheduleAppointment();

                DateTime dt = (DateTime)DatePicker.Value;

                
                TimeSpan time = ((DateTime)TimePicker.Value).TimeOfDay;

                // Combine date and time

                ap.StartTime = TimePicker.Value.Value;
                ap.Subject = $"{SelectedCustomer.FullName} Scheduled Event";
                ap.EndTime = selectedTime;
                ap.Notes = $"Number of golfers: {txtNumOfGolfers.Text}";
                ap.AppointmentBackground = Brushes.Green;

                CourseReservation cr = new CourseReservation();

                if (txtCourse.Text == "East") {
                    cr.AreaReserved = 1;
                }
                else if (txtCourse.Text == "South") {
                    cr.AreaReserved = 2;
                }
                else if (txtCourse.Text == "West") {
                    cr.AreaReserved = 3;
                }

                cr.Reservation = ap;
                SQLDataService sq = new SQLDataService(DataConstants.Keys.ConnectionString);

                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter("@ReservationArea", cr.AreaReserved));
                parameters.Add(new SqlParameter("@ScheduleAppointment", JsonConvert.SerializeObject(cr.Reservation)));
                parameters.Add(new SqlParameter("@CustomerUid", SelectedCustomer.Uid));

                string query = @"INSERT INTO CourseReservations (ReservationArea, ScheduleAppointment, CustomerUid) 
                 VALUES (@ReservationArea, @ScheduleAppointment, @CustomerUid);";


                sq.ExecuteNonQueryWithParams(query, parameters);

                LoadAppointments();

            }
        }
    }
}
