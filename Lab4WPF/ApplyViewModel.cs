using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lab4WPF
{
    public class ApplyViewModel : INotifyPropertyChanged
    {
        public static UdpClient udpClient;
        public ObservableCollection<Ability> Abilities 
        {
            get;
            set;
        }

        public Ability selectedAbility;
        public string result;

        private string nameadd;
        private string descadd;
        private string manacostadd;
        private string damageadd;
        private string reloadadd;
        private string idfind;
        public ApplyViewModel()
        {
            udpClient = new UdpClient(15002);
            UpdateAbilities();
        }

        public string Nameadd
        {
            get
            {
                return nameadd;
            }
            set
            {
                nameadd = value;
                OnPropertyChanged("Nameadd");
            }
        }

        public string Descadd
        {
            get
            {
                return descadd;
            }
            set
            {
                descadd = value;
                OnPropertyChanged("Descadd");
            }
        }

        public string Manacostadd
        {
            get
            {
                return manacostadd;
            }
            set
            {
                manacostadd = value;
                OnPropertyChanged("Manacostadd");
            }
        }

        public string Damageadd
        {
            get
            {
                return damageadd;
            }
            set
            {
                damageadd = value;
                OnPropertyChanged("Damageadd");
            }
        }

        public string Reloadadd
        {
            get
            {
                return reloadadd;
            }
            set
            {
                reloadadd = value;
                OnPropertyChanged("Reloadadd");
            }
        }

        public string IDFind
        {
            get
            {
                return idfind;
            }
            set
            {
                idfind = value;
                OnPropertyChanged("IDFind");
            }
        }

        public bool UpdateAbility
        {
            get
            {
                return false;
            }
            set
            {
                UpdateAbilities();

            }
        }
        public bool AddAbility
        {
            get
            {
                return false;
            }
            set
            {
                AddAbilities();

            }
        }

        public bool DeleteAbility
        {
            get
            {
                return false;
            }
            set
            {
                if (selectedAbility != null)
                    DeleteAbilities();
                else
                    Result = "Элемент не выбран";
            }
        }

        public bool FindAbility
        {
            get
            {
                return false;
            }
            set
            {
                if (int.TryParse(idfind, out int s))
                    FindAbilities();
                else
                    Result = "Элемент не выбран";
            }
        }

        public string Result
        {
            get
            {
                return result;
            }
            set
            {
                result = value;
                OnPropertyChanged("Result");
            }
        }

        public Ability SelectedAbility
        {
            get
            {
                return selectedAbility;
            }
            set
            {
                selectedAbility = value;
                OnPropertyChanged("SelectedAbility");
            }
        }
        public void UpdateAbilities()
        {
            if (Abilities == null)
                Abilities = new ObservableCollection<Ability>();
            else
                Abilities.Clear();
            SendMessage("1");
            string message = RecieveMessage();
            string[] mes = message.Split('\n');
            foreach (var m in mes)
            {
                string[] m2 = m.Split(';');
                int dam = 0;
                int mana = 0;
                int rel = 0;
                int id = 0;
                if (int.TryParse(m2[0], out id) && int.TryParse(m2[3], out mana) && int.TryParse(m2[4], out dam) && int.TryParse(m2[5], out rel))
                {
                    Ability ab = new Ability { ID = id, Name = m2[1], Desc = m2[2], Manacost = mana, Damage = dam, Reloadinsec = rel };
                    Abilities.Add(ab);
                }
            }
        }

        public void AddAbilities()
        {
            string add = $"{nameadd};{descadd};{manacostadd};{damageadd};{reloadadd}";
            add = add.Replace(" ", "_");
            SendMessage("4 " + add);
            string message = RecieveMessage();
            Result = message;
            UpdateAbilities();
            Nameadd = "";
            Descadd = "";
            Manacostadd = "";
            Damageadd = "";
            Reloadadd = "";
        }

        public void DeleteAbilities()
        {
            SendMessage("3 " + SelectedAbility.ID);
            string message = RecieveMessage();
            Result = message;
            UpdateAbilities();
        }

        public void FindAbilities()
        {
            Abilities.Clear();
            SendMessage("2 " + idfind);
            string message = RecieveMessage();
            string[] m2 = message.Split(';');
            int dam = 0;
            int mana = 0;
            int rel = 0;
            int id = 0;
            if (int.TryParse(m2[0], out id) && int.TryParse(m2[3], out mana) && int.TryParse(m2[4], out dam) && int.TryParse(m2[5], out rel))
            {
                Ability ab = new Ability { ID = id, Name = m2[1], Desc = m2[2], Manacost = mana, Damage = dam, Reloadinsec = rel };
                Abilities.Add(ab);
            }
            IDFind = "";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }


        private static void SendMessage(string message)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(message);
                udpClient.Send(data, data.Length, "127.0.0.1", 15003);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static string RecieveMessage()
        {
            IPEndPoint remoteIP = (IPEndPoint)udpClient.Client.LocalEndPoint;
            try
            {
                byte[] data = udpClient.Receive(ref remoteIP);
                string message = Encoding.UTF8.GetString(data);
                return message;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
