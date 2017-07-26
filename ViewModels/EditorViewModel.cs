using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Ping_Monitor
{
    public class EditorViewModel : INotifyPropertyChanged
    {
        public EditorViewModel(Pinger pinger)
        {
            Title = String.Format($"{pinger.Address} - Edit");
            this.pinger = pinger;
        }

        public string Title { get; set; }

        Pinger pinger;
        public Pinger Pinger
        {
            get { return pinger; }
            set
            {
                pinger = value;
                OnPropertyChanged("Pinger");
            }
        }

        private RelayCommand reset_Command;
        public RelayCommand Reset_Command
        {
            get
            {
                return reset_Command ??
                (reset_Command = new RelayCommand(obj =>
                {
                    pinger.Status = "";
                    pinger.Total = 0;
                    pinger.Lost_Count = 0;
                    pinger.Lost = 0;
                    pinger.Error_Count = 0;
                    pinger.Time_Total = 0;
                    pinger.Time_Avg = 0;
                    pinger.Time_Max = 0;
                    pinger.Time_Min = 0;
                    pinger.Time_Cur = 0;
                }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }


    }
}
