using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lab4WPF
{
    public class Ability : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private string desc;
        private int manacost;
        private int damage;
        private int reloadinsec;

        public int ID 
        { 
            get { return id; } 
            set
            {
                id = value;
                OnPropertyChanged("ID");
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

        public string Desc
        {
            get { return desc; }
            set
            {
                desc = value;
                OnPropertyChanged("Desc");
            }
        }

        public int Manacost
        {
            get { return manacost; }
            set
            {
                manacost = value;
                OnPropertyChanged("Manacost");
            }
        }

        public int Damage
        {
            get { return damage; }
            set
            {
                damage = value;
                OnPropertyChanged("Damage");
            }
        }

        public int Reloadinsec
        {
            get { return reloadinsec; }
            set
            {
                reloadinsec = value;
                OnPropertyChanged("Reloadinsec");
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
