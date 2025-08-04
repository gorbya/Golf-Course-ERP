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
    /// Interaction logic for ReportUserControl.xaml
    /// </summary>
    public partial class ReportUserControl : UserControl {
        public ReportUserControl() {
            InitializeComponent();
        }

        private void GenerateReport_Click(object sender, RoutedEventArgs e) {
            string title = TitleTextBox.Text;
            string reportType = ((ComboBoxItem)ReportTypeComboBox.SelectedItem)?.Content.ToString();
            DateTime? startDate = StartDatePicker.SelectedDate;
            DateTime? endDate = EndDatePicker.SelectedDate;

            if (string.IsNullOrWhiteSpace(title) || reportType == null || !startDate.HasValue || !endDate.HasValue) {
                MessageBox.Show("Please complete all fields.", "Input Required", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            var reportData = new List<ReportRow>();

            foreach (var loadProduct in LoadProducts()) {
                ReportRow rr = new ReportRow() {
                    Id = loadProduct.Uid,
                    Name = loadProduct.Name,
                    Value = loadProduct.Price
                };

                reportData.Add(rr);
            }

            ReportDataGrid.ItemsSource = reportData;
        }


        public class ReportRow {
            public int Id { get; set; }
            public string Name { get; set; }
            public double Value { get; set; }
        }

        private ObservableCollection<Concession> LoadProducts() {
            //Placeholder 

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

            return concessionList;
        }
    }
}
