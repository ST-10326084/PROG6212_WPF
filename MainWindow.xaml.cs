using System.Windows;
using PROG6212_WPF.ViewModels; // Ensure correct using directive

namespace PROG6212_WPF // Ensure correct namespace matches XAML
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
