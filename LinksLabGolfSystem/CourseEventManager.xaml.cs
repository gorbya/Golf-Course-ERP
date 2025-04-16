using LinksLabGolfSystem.Models;
using SqlHelper;
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

namespace LinksLabGolfSystem {
    /// <summary>
    /// Interaction logic for CourseEventManager.xaml
    /// </summary>
    public partial class CourseEventManager : UserControl, INotifyPropertyChanged
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

        public CourseEventManager() {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NMaF5cXmBCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWX1fd3ZWRmleUUZ0WUs=");
            InitializeComponent();
            DataContext = this;
            GetCustomers();
            grdCustomers.ItemsSource = Customers;
        }

        public void GetCustomers() {
            SQLDataService sdq = new SQLDataService(DataConstants.SQL.ConnectionString);

            DataTable dt = sdq.ExecuteSelectQuery("Select * from Customers");
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

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null) {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
