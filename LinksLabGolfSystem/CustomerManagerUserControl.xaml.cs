using LinksLabGolfSystem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SqlHelper;
using Syncfusion.Windows.Primitives;

namespace LinksLabGolfSystem {
    /// <summary>
    /// Interaction logic for CustomerManagerUserControl.xaml
    /// </summary>
    public partial class CustomerManagerUserControl : UserControl, INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Customer> _Customers;
        private Customer _SelectedCustomer;
        private SolidColorBrush _CustomerEditGuiColor;
        private bool _TextBoxesEnabled;

        public bool TextBoxesEnabled {
            get {
                return _TextBoxesEnabled;
            }
            set{
                _TextBoxesEnabled = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush CustomerEditGuiColor {
            get {
                if (_CustomerEditGuiColor == null) {
                    _CustomerEditGuiColor = new SolidColorBrush(Colors.White);
                }

                return _CustomerEditGuiColor;
            }
            set {
                _CustomerEditGuiColor = value;
                OnPropertyChanged();
            }
        }

        public Customer SelectedCustomer {
            get {
                if (_SelectedCustomer == null) {
                    _SelectedCustomer = new Customer();
                }
                return _SelectedCustomer;
            }
            set {
                _SelectedCustomer = value;
                if (_SelectedCustomer != null) {
                    if (_SelectedCustomer.Uid > 0) {
                        btnCancelNewCustomer.Visibility = Visibility.Hidden;
                        btnDeleteCustomer.IsEnabled = true;
                        CustomerEditGuiColor = new SolidColorBrush(Colors.White);
                        btnDeleteCustomer.Visibility = Visibility.Visible;
                        TextBoxesEnabled = true;
                    }
                    else if (_SelectedCustomer.Uid == 0) {
                        TextBoxesEnabled = false;
                    }
                    else {
                        TextBoxesEnabled = true;
                        btnDeleteCustomer.Visibility = Visibility.Hidden;
                    }
                }

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
            //Clear our current list
            Customers.Clear();

            //Connect to our SQL DB and cast to the Customer object
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

            //Build our new list
            Customers = customerList;
        }

        private void BtnSaveCustomer_OnClick(object sender, RoutedEventArgs e) {

            if (SelectedCustomer.FirstName == null) {
                MessageBox.Show("Please Enter a Name");
                return;
            }
            if (SelectedCustomer.Uid > 0) {

                //SqlParameter uidParameter = new SqlParameter();
                List<SqlParameter> parameters = new List<SqlParameter> {
                    new SqlParameter("@FirstName", SelectedCustomer.FirstName),
                    new SqlParameter("@LastName", SelectedCustomer.LastName),
                    new SqlParameter("@Email", SelectedCustomer.Email),
                    new SqlParameter("@IsMember", SelectedCustomer.IsMember),
                    new SqlParameter("@RenewalDate", SelectedCustomer.RenewalDate ?? (object)DBNull.Value),
                    new SqlParameter("@Uid", SelectedCustomer.Uid)
                };

                string query = "UPDATE Customers SET FirstName = @FirstName, LastName = @LastName, Email = @Email, IsMember = @IsMember, RenewalDate = @RenewalDate WHERE Uid = @Uid";

                SQLDataService sds = new SQLDataService(DataConstants.Keys.ConnectionString);

                sds.ExecuteNonQueryWithParams(query, parameters);
            }

            else if (SelectedCustomer.Uid < 0) {
                List<SqlParameter> parameters = new List<SqlParameter> {
                    new SqlParameter("@FirstName", SelectedCustomer.FirstName),
                    new SqlParameter("@LastName", SelectedCustomer.LastName),
                    new SqlParameter("@Email", SelectedCustomer.Email),
                    new SqlParameter("@IsMember", SelectedCustomer.IsMember),
                    new SqlParameter("@RenewalDate", SelectedCustomer.RenewalDate ?? (object)DBNull.Value),
                    new SqlParameter("@IsDeleted", false)
                };

                string query = "INSERT INTO Customers (FirstName, LastName, Email, IsMember, RenewalDate, IsDeleted) VALUES (@FirstName, @LastName, @Email, @IsMember, @RenewalDate, @IsDeleted)";

                SQLDataService sds = new SQLDataService(DataConstants.Keys.ConnectionString);

                sds.ExecuteNonQueryWithParams(query, parameters);
            }

            CustomerEditGuiColor = new SolidColorBrush(Colors.White);
            btnCancelNewCustomer.Visibility = Visibility.Hidden;
            GetCustomers();
            if (Customers.Count > 1) {
                SelectedCustomer = Customers[0];
            }

        }

        private void BtnCreateNewCustomer_OnClick(object sender, RoutedEventArgs e) {
            //New up a customer

            SelectedCustomer = new Customer();
            SelectedCustomer.Uid = -1;
            TextBoxesEnabled = true;
            CustomerEditGuiColor = new SolidColorBrush(Colors.LightGreen);
            btnDeleteCustomer.IsEnabled = false;
            btnCancelNewCustomer.Visibility = Visibility.Visible;

        }

        private void BtnCancelNewCustomer_OnClick(object sender, RoutedEventArgs e) {
            SelectedCustomer = Customers[0];
            CustomerEditGuiColor = new SolidColorBrush(Colors.White);
            btnCancelNewCustomer.Visibility = Visibility.Hidden;
        }

        private void BtnDeleteCustomer_OnClick(object sender, RoutedEventArgs e) {
            MessageBoxResult result = MessageBox.Show(
                "Do you want to delete this Customer?",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Information
            );

            if (result == MessageBoxResult.Yes) {
                List<SqlParameter> parameters = new List<SqlParameter> {
                    new SqlParameter("@CustomerUid", SelectedCustomer.Uid)
                };

                string query = "UPDATE Customers SET IsDeleted = 1 WHERE Uid = @CustomerUid";

                SQLDataService sds = new SQLDataService(DataConstants.Keys.ConnectionString);
                sds.ExecuteNonQueryWithParams(query, parameters);

                GetCustomers();
            }
        }


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
