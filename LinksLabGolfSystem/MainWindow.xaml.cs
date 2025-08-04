using System.Windows;
using MahApps.Metro.Controls;

namespace LinksLabGolfSystem {
    public partial class MainWindow : MetroWindow {
        public MainWindow() {
            InitializeComponent();

            // Initially show the Login UserControl
            MainContent.Content = new LoginUserControl();
        }

        // Home Button Click
        private void HomeButton_Click(object sender, RoutedEventArgs e) {
            // Load Home UserControl into MainContent
            MainContent.Content = new HomeUserControl();
        }

        // Products Button Click
        private void CheckoutButton_Click(object sender, RoutedEventArgs e) {
            // Load Products UserControl into MainContent
            MainContent.Content = new ConcessionSaleUserControl();
        }

        // Orders Button Click
        private void OrdersButton_Click(object sender, RoutedEventArgs e) {
            // Load Orders UserControl into MainContent
            //MainContent.Content = new OrdersUserControl();
        }
    }
}