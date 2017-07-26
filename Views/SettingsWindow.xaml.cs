using System.Windows;

namespace Ping_Monitor
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow(Settings settings)
        {
            InitializeComponent();
            DataContext = new SettingsViewModel(settings);
        }
    }
}
