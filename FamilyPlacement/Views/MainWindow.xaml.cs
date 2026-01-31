using FamilyPlacement.ViewModels;
using System.Windows;

namespace FamilyPlacement.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel mainWindowViewModel)
        {
            DataContext = mainWindowViewModel;
            InitializeComponent();
        }
    }
}
