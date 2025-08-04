using LinksLabGolfSystem.Models;
using SqlHelper;
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
using LinksLabGolfSystem.Models.Concessions;
using Syncfusion.UI.Xaml.Scheduler;

namespace LinksLabGolfSystem {
    /// <summary>
    /// Interaction logic for ConcessionSaleUserControl.xaml
    /// </summary>
    public partial class ConcessionSaleUserControl : UserControl {
        public ObservableCollection<Concession> Products { get; set; }
        public List<CartItem> Cart { get; set; } = new List<CartItem>();
        public ConcessionSaleUserControl() {
            InitializeComponent();

            LoadProducts();
        }

        public ConcessionSaleUserControl(ScheduleAppointment appointment) {
            InitializeComponent();

            LoadProducts();

            Concession teeTime = new Concession();
            teeTime.Name = appointment.Notes;
            teeTime.Price = 25.00;

            Cart.Add(new CartItem() {
                Product = teeTime, Quantity = 1
            });
            RefreshCart();
        }




        private void LoadProducts() {

            SQLDataService sdq = new SQLDataService(DataConstants.Keys.ConnectionString);

            DataTable dt = sdq.ExecuteSelectQuery("Select * from Concessions");
            ObservableCollection<Concession> concessionList = new ObservableCollection<Concession>();
            foreach (DataRow row in dt.Rows) {
                Concession cons = new Concession {
                    Uid = Convert.ToInt32(row["Uid"]),
                    Name = row["Name"].ToString(),
                    Price = Convert.ToDouble(row["Price"])
                };
                concessionList.Add(cons);
            }

            dt = sdq.ExecuteSelectQuery("Select * from Equipment where IsCustomerRentable = 1 AND IsAvailable = 1");
            ObservableCollection<Concession> eqptList = new ObservableCollection<Concession>(); //Needs to be refactored to not be a concession :) 
            foreach (DataRow row in dt.Rows) {
                Concession cons = new Concession {
                    Uid = Convert.ToInt32(row["Uid"]),
                    Name = row["Description"].ToString(),
                    Price = 14.99
                };
                concessionList.Add(cons);


                //Needs some logic here to tell the equipment table that the item is rented out and unavailable...
                //Version 2 maybe :) 
            }

            Products = concessionList;
            
            ProductListBox.ItemsSource = Products;
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e) {
            if (ProductListBox.SelectedItem is Concession product && int.TryParse(QuantityTextBox.Text, out int qty)) {
                var existing = Cart.FirstOrDefault(c => c.Product.Name == product.Name);
                if (existing != null) {
                    existing.Quantity += qty;
                }
                else {
                    Cart.Add(new CartItem { Product = product, Quantity = qty });
                }

                RefreshCart();
            }
        }

        private void RemoveFromCart_Click(object sender, RoutedEventArgs e) {
            if (CartListBox.SelectedItem is CartItem selected) {
                Cart.Remove(selected);
                RefreshCart();
            }
        }

        private void Checkout_Click(object sender, RoutedEventArgs e) {
            double total = Cart.Sum(item => item.Product.Price * item.Quantity);
            MessageBox.Show($"Total: ${total:0.00}", "Checkout");
            Cart.Clear();
            RefreshCart();
        }

        private void RefreshCart() {
            CartListBox.ItemsSource = null;
            CartListBox.ItemsSource = Cart;
        }
    }

    public class Product {
        public string Name { get; set; }
        public double Price { get; set; }
    }

    public class CartItem {
        public Concession Product { get; set; }
        public int Quantity { get; set; }

        public string Display => $"{Product.Name} x{Quantity} - ${Product.Price * Quantity:0.00}";
    }
}