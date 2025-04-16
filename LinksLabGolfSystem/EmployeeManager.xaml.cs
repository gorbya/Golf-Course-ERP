using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace LinksLabGolfSystem {
    /// <summary>
    /// Interaction logic for EmployeeManager.xaml
    /// </summary>
    public partial class EmployeeManager : UserControl
    {
        private ObservableCollection<string> _employees = new ObservableCollection<string>() {
                "Ash",
                "Anisa",
                "Ben",
                "Beth",
                "Carl"
            };

        public ObservableCollection<string> Employees {
            get => _employees;
            set => _employees = value;
        }


        public EmployeeManager() {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NMaF5cXmBCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWX1fd3ZWRmleUUZ0WUs=");
            DataContext = this;
            InitializeComponent();
        }
    }
}
