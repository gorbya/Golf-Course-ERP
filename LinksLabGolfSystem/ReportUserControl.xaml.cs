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
                MessageBox.Show("Please complete all fields.", "Input Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var reportData = new List<ReportRow>
            {
                new ReportRow { Id = 1, Name = "Item A", Value = 123.45 },
                new ReportRow { Id = 2, Name = "Item B", Value = 678.90 },
                new ReportRow { Id = 3, Name = "Item C", Value = 234.56 }
            };

            ReportDataGrid.ItemsSource = reportData;
        }
    }

    public class ReportRow {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
    }
}
