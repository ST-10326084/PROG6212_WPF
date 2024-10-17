using System.Windows;
using PROG6212_WPF.ViewModels;

namespace PROG6212_WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel(); // Set the DataContext to the MainWindowViewModel
        }
    }
}
