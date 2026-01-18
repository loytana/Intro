using System.Windows;
using WallOpening.ViewModels;

namespace WallOpening.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindow mainWindow)
        {     
            this.DataContext = mainWindow;
            InitializeComponent();
        }
    }
}
