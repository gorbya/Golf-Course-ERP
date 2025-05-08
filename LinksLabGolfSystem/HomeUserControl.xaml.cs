using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
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
using LinksLabGolfSystem.Models;
using LinksLabGolfSystem;
using Syncfusion.UI.Xaml.Scheduler;

namespace LinksLabGolfSystem {
    /// <summary>
    /// Interaction logic for HomeUserControl.xaml
    /// </summary>
    public partial class HomeUserControl : UserControl, INotifyPropertyChanged {

        public HomeUserControl() {
            InitializeComponent();
        }

        public HomeUserControl(int security) {
            InitializeComponent();
            if (security != 1) {
                btnCustomerMgr.Visibility = Visibility.Hidden;
                btnCustomerMgr.IsEnabled = false;
                btnEmployeeMgr.Visibility = Visibility.Hidden;
                btnEmployeeMgr.IsEnabled = false;
            }


            Scheduler.CheckoutRequested += OnCheckoutRequested;

            //MainContent.Content = new ConcessionSaleUserControl();
        }

        private void BtnCustomerMgr_OnClick(object sender, RoutedEventArgs e) {
            MainContent.Content = new CustomerManagerUserControl();
        }

        private void OnCheckoutRequested(object sender, ScheduleAppointment appointment) {
            MainContent.Content = null; // Removes it
            int i = 1;
            MainContent.Content = new ConcessionSaleUserControl(appointment);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null) {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private void BtnOverview_OnClick(object sender, RoutedEventArgs e) {
            MainContent.Content = new Scheduler();
        }

        private void BtnEquipmentMgr_OnClick(object sender, RoutedEventArgs e) {
            MainContent.Content = new EquipmentManager();
        }

        private void BtnEmployeeMgr_OnClick(object sender, RoutedEventArgs e) {
            MainContent.Content = new EmployeeManager();
        }

        private void BtnEventMgr_OnClick(object sender, RoutedEventArgs e) {
            MainContent.Content = new CourseEventManager();
        }
    }
}
