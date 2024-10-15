using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PROG6212_WPF.Commands;
using PROG6212_WPF.ViewModels;

namespace CLVD6212_WPF.Views
{
    /// <summary>
    /// Interaction logic for DashboardView.xaml
    /// </summary>
    public partial class DashboardView : UserControl
    {
        public DashboardView()
        {
            InitializeComponent();
            DataContext = new DashboardViewModel(); // Set the DataContext to the ViewModel
        }
    }
}

