using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using LinksLabGolfSystem.DataConstants;
using LinksLabGolfSystem.Models;
using SqlHelper;
using Syncfusion.Licensing;

namespace LinksLabGolfSystem {
    /// <summary>
    ///     Interaction logic for EmployeeManager.xaml
    /// </summary>
    public partial class EquipmentManager : UserControl, INotifyPropertyChanged {



        private ObservableCollection<Equipment> _Equipments;
        private Equipment _SelectedEquipment;

        public EquipmentManager() {
            SyncfusionLicenseProvider.RegisterLicense(DataConstants.Keys.SyncFusionKey);
            DataContext = this;
            InitializeComponent();

            GetEquipment();

            grdEquipment.ItemsSource = Equipments;
        }

        public ObservableCollection<Equipment> Equipments {
            get {
                if (_Equipments == null) _Equipments = new ObservableCollection<Equipment>();

                return _Equipments;
            }
            set => _Equipments = value;
        }

        public Equipment SelectedEquipment {
            get {
                if (_SelectedEquipment == null) _SelectedEquipment = new Equipment();
                return _SelectedEquipment;
            }
            set {
                _SelectedEquipment = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void GetEquipment() {
            Equipments.Clear();

            var sdq = new SQLDataService(Keys.ConnectionString);

            var dt = sdq.ExecuteSelectQuery("Select * from Equipment");
            var equipmentList = new ObservableCollection<Equipment>();
            foreach (DataRow row in dt.Rows) {
                var equip = new Equipment();

                equip.Uid = (int)row["Uid"];
                equip.Description = (string)row["Description"];
                equip.MaintenanceFrequency = (string)row["MaintenanceFrequency"];
                equip.IsAvailable = Convert.ToBoolean(row["IsAvailable"]);

                if (row["LastDateMaintained"] == DBNull.Value) {
                    equip.LastDateMaintained = DateTime.MinValue;
                }

                else {
                    equip.LastDateMaintained = Convert.ToDateTime(row["LastDateMaintained"]);
                }

                equipmentList.Add(equip);
            }

            Equipments = equipmentList;
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

        private void BtnSaveEquipment_OnClick(object sender, RoutedEventArgs e) {
            if (SelectedEquipment != null) {
                SQLDataService sds = new SQLDataService(DataConstants.Keys.ConnectionString);

                List<SqlParameter> parameters = new List<SqlParameter> {
                    new SqlParameter("@Uid", SelectedEquipment.Uid),
                    new SqlParameter("@Description", SelectedEquipment.Description),
                    new SqlParameter("@MaintenanceFrequency", SelectedEquipment.MaintenanceFrequency),
                    new SqlParameter("@LastDate", DateTime.Now),
                    new SqlParameter("@IsAvailable", SelectedEquipment.IsAvailable)
                };

                string query = "UPDATE Equipment SET Description = @Description, MaintenanceFrequency = @MaintenanceFrequency, LastDateMaintained = @LastDate, IsAvailable = @IsAvailable WHERE Uid = @Uid";

                sds.ExecuteNonQueryWithParams(query, parameters);
            }
        }
    }
}