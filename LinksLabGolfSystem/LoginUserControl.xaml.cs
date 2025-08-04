using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using LinksLabGolfSystem.Models;
using Microsoft.Xaml.Behaviors.Media;
using SqlHelper;

namespace LinksLabGolfSystem {
    public partial class LoginUserControl : UserControl {
        public LoginUserControl() {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e) {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;


            SQLDataService sqlData = new SQLDataService(DataConstants.Keys.ConnectionString);

            List<SqlParameter> parameters = new List<SqlParameter>();

            string encryptedUsername = DataEncryptor.Encrypt(username, DataConstants.Keys.EncryptionKey);
            string encryptedPassword = DataEncryptor.Encrypt(password, DataConstants.Keys.EncryptionKey);

            parameters.Add(new SqlParameter("@Username", encryptedUsername));
            parameters.Add(new SqlParameter("@Password", encryptedPassword));

            string query = "SELECT * from [User] where Username = @Username AND Password = @Password";

            User loggedUser = null;

            using (SqlConnection connection = new SqlConnection(DataConstants.Keys.ConnectionString)) {
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddRange(parameters.ToArray());
                connection.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader()) {
                    if (sdr.Read()) {
                        loggedUser = new User();
                        loggedUser.Username = sdr.GetString(0);
                        loggedUser.Password = sdr.GetString(1);
                        loggedUser.AppliedSecurity = sdr.GetInt32(2);
                    }
                }
            }

            if (loggedUser != null) {

                // Hide the login form and display main content
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

                if (loggedUser.AppliedSecurity == 1) {
                    mainWindow.CheckoutButton.IsEnabled = true;
                    mainWindow.CheckoutButton.Visibility = Visibility.Visible;
                }
                mainWindow.HomeButton.IsEnabled = true;

                mainWindow.HomeButton.Visibility = Visibility.Visible;

                mainWindow.MainContent.Content = new HomeUserControl(loggedUser.AppliedSecurity);
            }

            else {
                    // Show error message if login fails
                    ErrorTextBlock.Text = "Invalid username or password.";
                    ErrorTextBlock.Visibility = Visibility.Visible;
            }
        }
    }
}