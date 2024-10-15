using System.Windows;
using PROG6212_WPF.ViewModels;

public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Set the DataContext to the MainWindowViewModel
            this.DataContext = new MainWindowViewModel();
        }
    }
