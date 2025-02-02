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
using LinksLabGolfSystem.SQL;

namespace LinksLabGolfSystem {
    /// <summary>
    /// Interaction logic for HomeUserControl.xaml
    /// </summary>
    public partial class HomeUserControl : UserControl, INotifyPropertyChanged
    {

        private ObservableCollection<Customer> _Customers;

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

        public HomeUserControl() {
            InitializeComponent();
        }

        private void BtnCustomerMgr_OnClick(object sender, RoutedEventArgs e) {
            SQLDataService sdq = new SQLDataService();

            DataTable dt = sdq.ExecuteSelectQuery("Select * from Customers");
            ObservableCollection<Customer> customerList = new ObservableCollection<Customer>();
            foreach (DataRow row in dt.Rows)
            {
                // Create a new Customer object for each row.
                Customer customer = new Customer
                {
                    // Use Convert or safe casting as needed.
                    Uid = Convert.ToInt32(row["Uid"]),
                    Name = row["Name"].ToString(),
                    Email = row["Email"].ToString(),
                    IsMember = (bool)row["IsMember"],
                    RenewalDate = (DateTime)row["RenewalDate"]
                    // Map additional columns to properties here.
                };

            }

            // Add the newly created customer object to the list.
            Customers = customerList;
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
    }
}
