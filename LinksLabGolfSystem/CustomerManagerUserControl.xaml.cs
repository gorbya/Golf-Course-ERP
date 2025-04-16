using LinksLabGolfSystem.Models;
using LinksLabGolfSystem;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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
using SqlHelper;

namespace LinksLabGolfSystem {
    /// <summary>
    /// Interaction logic for CustomerManagerUserControl.xaml
    /// </summary>
    public partial class CustomerManagerUserControl : UserControl, INotifyPropertyChanged {
        private ObservableCollection<Customer> _Customers;
        private Customer _SelectedCustomer;

        public Customer SelectedCustomer {
            get {
                if (_SelectedCustomer == null) {
                    _SelectedCustomer = new Customer();
                }
                return _SelectedCustomer;
            }
            set
            {
                _SelectedCustomer = value;
                OnPropertyChanged();
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

        public CustomerManagerUserControl() {
            DataContext = this;
            InitializeComponent();
            

            GetCustomers();

            grdCustomers.ItemsSource = Customers;
        }

        public void GetCustomers() {

            Customers.Clear();

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

        private void BtnSaveCustomer_OnClick(object sender, RoutedEventArgs e) {
            if (SelectedCustomer.Uid > 0) {

                //SqlParameter uidParameter = new SqlParameter();
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add( new SqlParameter("@FirstName", SelectedCustomer.FirstName));
                parameters.Add(new SqlParameter("@LastName", SelectedCustomer.LastName));
                parameters.Add(new SqlParameter("@Email", SelectedCustomer.Email));
                parameters.Add(new SqlParameter("@IsMember", SelectedCustomer.IsMember));
                parameters.Add(new SqlParameter("@RenewalDate", SelectedCustomer.RenewalDate ?? (object)DBNull.Value));
                parameters.Add(new SqlParameter("@Uid", SelectedCustomer.Uid));

                string query = "UPDATE Customers SET FirstName = @FirstName, LastName = @LastName, Email = @Email, IsMember = @IsMember, RenewalDate = @RenewalDate WHERE Uid = @Uid";

                SQLDataService sds = new SQLDataService(DataConstants.SQL.ConnectionString);

                sds.ExecuteNonQueryWithParams(query, parameters);
            }

            else {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter("@FirstName", SelectedCustomer.FirstName));
                parameters.Add(new SqlParameter("@LastName", SelectedCustomer.LastName));
                parameters.Add(new SqlParameter("@Email", SelectedCustomer.Email));
                parameters.Add(new SqlParameter("@IsMember", SelectedCustomer.IsMember));
                parameters.Add(new SqlParameter("@RenewalDate", SelectedCustomer.RenewalDate ?? (object)DBNull.Value));

                string query = "INSERT INTO Customers (FirstName, LastName, Email, IsMember, RenewalDate) VALUES (@FirstName, @LastName, @Email, @IsMember, @RenewalDate)";

                SQLDataService sds = new SQLDataService(DataConstants.SQL.ConnectionString);

                sds.ExecuteNonQueryWithParams(query, parameters);
            }

            GetCustomers();

        }

        private void BtnCreateNewCustomer_OnClick(object sender, RoutedEventArgs e) {
            SelectedCustomer = new Customer();
            SelectedCustomer.Uid = -1;
        }
    }
}
