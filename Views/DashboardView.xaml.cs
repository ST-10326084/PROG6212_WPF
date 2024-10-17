using PROG6212_WPF.ViewModels;
using System.Windows.Controls;

namespace PROG6212_WPF.Views
{
    public partial class DashboardView : UserControl
    {
        public DashboardView()
        {
            InitializeComponent(); // Ensure this is present and matches the XAML
            DataContext = new DashboardViewModel(); // Set the DataContext to the ViewModel
        }
    }
}
