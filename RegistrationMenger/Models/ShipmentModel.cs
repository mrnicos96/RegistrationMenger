
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RegistrationMenger.Models
{

    public class Shipment : INotifyPropertyChanged
    {
        private string id;
        private string name;
        private int qte;

        public string Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public int Qte
        {
            get { return qte; }
            set
            {
                qte = value;
                OnPropertyChanged("Qte");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
