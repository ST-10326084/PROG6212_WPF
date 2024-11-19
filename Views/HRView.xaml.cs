using PROG6212_WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PROG6212_WPF.Views
{
    /// <summary>
    /// Interaction logic for HRView.xaml
    /// </summary>
    public partial class HRView : UserControl
    {
        public HRView()
        {
            InitializeComponent();
            InitializeComponent(); // Ensure this is present and matches the XAML
            DataContext = new HRViewModel(); // Set the DataContext to the ViewModel
        }
    }
}
