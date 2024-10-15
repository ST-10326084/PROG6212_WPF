using System.Windows.Controls;
using PROG6212_WPF.ViewModels;

namespace PROG6212_WPF.Views
{
    /// <summary>
    /// Interaction logic for VerifyClaimsView.xaml
    /// </summary>
    public partial class VerifyClaimsView : UserControl
    {
        public VerifyClaimsView()
        {
            InitializeComponent();
            DataContext = new VerifyClaimsViewModel(); // Set DataContext to the ViewModel
        }
    }
}
