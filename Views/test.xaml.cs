using System.Windows.Controls;
using PROG6212_WPF.ViewModels;

namespace PROG6212_WPF.Views
{
    /// <summary>
    /// Interaction logic for test.xaml
    /// </summary>
    public partial class test : UserControl
    {
        public test()
        {
            InitializeComponent();
            DataContext = new testC(); // Set DataContext to the new ViewModel
        }
    }
}