using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace LinksLabGolfSystem {
    /// <summary>
    /// Interaction logic for TimePickerWindow.xaml
    /// </summary>
    public partial class TimePickerWindow : Window {
        public DateTime SelectedTime { get; private set; }

        public TimePickerWindow() {
            InitializeComponent();
        }

        private void Submit_Click(object sender, RoutedEventArgs e) {
            if (TimePicker.Value.HasValue) {
                SelectedTime = TimePicker.Value.Value;
                DialogResult = true;
                Close();
            }
        }
    }
}
