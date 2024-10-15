using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PROG6212_WPF.ViewModels;

namespace PROG6212_WPF.Views
{
    /// <summary>
    /// Interaction logic for SubmitClaimView.xaml
    /// </summary>
    public partial class SubmitClaimView : UserControl
    {
        public SubmitClaimView()
        {
            InitializeComponent();
            DataContext = new SubmitClaimViewModel(); // Set DataContext to the ViewModel
        }
    }
}
