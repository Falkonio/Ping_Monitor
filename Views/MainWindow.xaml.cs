using System.Windows;

namespace Ping_Monitor
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new PingerViewModel(new DefaultDialogService(), new JsonFileService(), this);
        }

    }
}
