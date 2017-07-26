using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Ping_Monitor
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        public SettingsViewModel(Settings settings)
        {
            this.settings = settings;
        }

        Settings settings;
        public Settings Settings
        {
            get { return settings; }
            set
            {
                settings = value;
                OnPropertyChanged("Settings");
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
