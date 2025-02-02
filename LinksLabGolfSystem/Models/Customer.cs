using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LinksLabGolfSystem.Models {
    public class Customer : INotifyPropertyChanged {
        public int Uid { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsMember { get; set; }
        public DateTime RenewalDate { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
