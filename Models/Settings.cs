using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Ping_Monitor
{

    public class Settings : INotifyPropertyChanged
    {
        private int timeout;
        private int pause;
        private bool allert_enable;
        private int allert_errors;
        private bool email_enable;
        private int email_errors;
        private string email_login;
        private string email_pass;
        private string email_server;
        private int email_port;
        private bool email_secure;
        private string email_address;
        private int email_pause;


        public int Timeout
        {
            get { return timeout; }
            set
            {
                timeout = value;
                OnPropertyChanged("Timeout");
            }
        }

        public int Pause
        {
            get { return pause; }
            set
            {
                pause = value;
                OnPropertyChanged("Pause");
            }
        }

        //Allert settings
        public bool Allert_Enable
        {
            get { return allert_enable; }
            set
            {
                allert_enable = value;
                OnPropertyChanged("Allert_Enable");
            }
        }
        public int Allert_Errors
        {
            get { return allert_errors; }
            set
            {
                allert_errors = value;
                OnPropertyChanged("Allert_Errors");
            }
        }

        //Email notifications settings
        public bool Email_Enable
        {
            get { return email_enable; }
            set
            {
                email_enable = value;
                OnPropertyChanged("Email_Enable");
            }
        }
        public int Email_Errors
        {
            get { return email_errors; }
            set
            {
                email_errors = value;
                OnPropertyChanged("Email_Errors");
            }
        }
        public int Email_Pause
        {
            get { return email_pause; }
            set
            {
                email_pause = value;
                OnPropertyChanged("Email_Pause");
            }
        }
        public string Email_Login
        {
            get { return email_login; }
            set
            {
                email_login = value;
                OnPropertyChanged("Email_Login");
            }
        }
        public string Email_Pass
        {
            get { return email_pass; }
            set
            {
                email_pass = value;
                OnPropertyChanged("Email_Pass");
            }
        }
        public string Email_Server
        {
            get { return email_server; }
            set
            {
                email_server = value;
                OnPropertyChanged("Email_Server");
            }
        }
        public int Email_Port
        {
            get { return email_port; }
            set
            {
                email_port = value;
                OnPropertyChanged("Email_Port");
            }
        }
        public bool Email_Secure
        {
            get { return email_secure; }
            set
            {
                email_secure = value;
                OnPropertyChanged("Email_Secure");
            }
        }
        public string Email_Address
        {
            get { return email_address; }
            set
            {
                email_address = value;
                OnPropertyChanged("Email_Address");
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
