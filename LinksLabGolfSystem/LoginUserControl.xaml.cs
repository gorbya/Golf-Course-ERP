using System.Windows;
using System.Windows.Controls;

namespace LinksLabGolfSystem {
    public partial class LoginUserControl : UserControl {
        public LoginUserControl() {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e) {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            // Simulated login check (replace with actual authentication logic)
            if (username == "admin" && password == "admin") {
                // Hide the login form and display main content (e.g., Home page)
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

                // Enable and show buttons in MainWindow
                mainWindow.HomeButton.IsEnabled = true;
                mainWindow.ConcessionButton.IsEnabled = true;

                mainWindow.HomeButton.Visibility = Visibility.Visible;
                mainWindow.ConcessionButton.Visibility = Visibility.Visible;

                // Optionally switch to a different user control, like Home page
                mainWindow.MainContent.Content = new HomeUserControl();
            }
            else {
                // Show error message if login fails
                ErrorTextBlock.Text = "Invalid username or password.";
                ErrorTextBlock.Visibility = Visibility.Visible;
            }
        }



        //private void LoginButton_Click(object sender, RoutedEventArgs e) {
        //    string username = UsernameTextBox.Text;
        //    string password = PasswordBox.Password;

        //    // Simulated login check (replace with actual authentication logic)
        //    if (username == "admin" && password == "password123") {
        //        // Hide the login form and display main content (e.g., Home page)
        //        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        //        mainWindow.MainContent.Content = new HomeUserControl();
        //    }
        //    else {
        //        // Show error message if login fails
        //        ErrorTextBlock.Text = "Invalid username or password.";
        //        ErrorTextBlock.Visibility = Visibility.Visible;
        //    }
        //}



    }
}