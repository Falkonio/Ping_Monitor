using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Ping_Monitor
{
    [DataContract]
    [Serializable]
    public class Pinger : INotifyPropertyChanged
    {
        [DataMember]
        private string name;
        [DataMember]
        private string address;
        [DataMember]
        private int ttl;
        [DataMember]
        private bool fragment;
        [DataMember]
        private bool log;
        [NonSerialized]
        private bool in_ping;
        [NonSerialized]
        private int total;
        [NonSerialized]
        private int lost_count;
        [NonSerialized]
        private int lost;
        [NonSerialized]
        private int error_count;
        [NonSerialized]
        private string status;
        [NonSerialized]
        private int ttl_return;
        [NonSerialized]
        private long time_cur;
        [NonSerialized]
        private long time_total;
        [NonSerialized]
        private long time_avg;
        [NonSerialized]
        private long time_max;
        [NonSerialized]
        private long time_min;
        [NonSerialized]
        private DateTime email_time;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Address
        {
            get { return address; }
            set
            {
                address = value;
                OnPropertyChanged("Address");
            }
        }

        public int TTL
        {
            get { return ttl; }
            set
            {
                ttl = value;
                OnPropertyChanged("TTL");
            }
        }

        public bool Fragment
        {
            get { return fragment; }
            set
            {
                fragment = value;
                OnPropertyChanged("Fragment");
            }
        }

        public bool Log
        {
            get { return log; }
            set
            {
                log = value;
                OnPropertyChanged("Log");
            }
        }

        public bool In_Ping
        {
            get { return in_ping; }
            set
            {
                in_ping = value;
                OnPropertyChanged("In_Ping");
            }
        }

        public int Total
        {
            get { return total; }
            set
            {
                total = value;
                OnPropertyChanged("Total");
            }
        }

        public int Lost_Count
        {
            get { return lost_count; }
            set
            {
                lost_count = value;
                OnPropertyChanged("Lost_Count");
            }
        }

        public int Lost
        {
            get { return lost; }
            set
            {
                lost = value;
                OnPropertyChanged("Lost");
            }
        }

        public int Error_Count
        {
            get { return error_count; }
            set
            {
                error_count = value;
                OnPropertyChanged("Error_Count");
            }
        }

        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        public int TTL_Return
        {
            get { return ttl_return; }
            set
            {
                ttl_return = value;
                OnPropertyChanged("TTL_Return");
            }
        }

        public long Time_Cur
        {
            get { return time_cur; }
            set
            {
                time_cur = value;
                OnPropertyChanged("Time_Cur");
            }
        }

        public long Time_Total
        {
            get { return time_total; }
            set
            {
                time_total = value;
                OnPropertyChanged("Time_Total");
            }
        }

        public long Time_Avg
        {
            get { return time_avg; }
            set
            {
                time_avg = value;
                OnPropertyChanged("Time_Avg");
            }
        }

        public long Time_Max
        {
            get { return time_max; }
            set
            {
                time_max = value;
                OnPropertyChanged("Time_Max");
            }
        }

        public long Time_Min
        {
            get { return time_min; }
            set
            {
                time_min = value;
                OnPropertyChanged("Time_Min");
            }
        }

        public DateTime Email_Time
        {
            get { return email_time; }
            set
            {
                email_time = value;
                OnPropertyChanged("Email_Time");
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
