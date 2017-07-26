using System.Windows;

namespace Ping_Monitor
{
    /// <summary>
    /// Логика взаимодействия для EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        public EditWindow(Pinger pinger)
        {
            InitializeComponent();
            DataContext = new EditorViewModel(pinger);
        }
    }
}
