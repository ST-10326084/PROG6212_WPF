using System.Windows.Input;
using System.ComponentModel;
using PROG6212_WPF.Commands;
using System.Windows;

namespace PROG6212_WPF.ViewModels
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        // Properties for the total counts
        private int _pendingClaimsCount;
        private int _approvedClaimsCount;
        private int _rejectedClaimsCount;

        public int PendingClaimsCount
        {
            get => _pendingClaimsCount;
            set
            {
                _pendingClaimsCount = value;
                OnPropertyChanged(nameof(PendingClaimsCount));
            }
        }

        public int ApprovedClaimsCount
        {
            get => _approvedClaimsCount;
            set
            {
                _approvedClaimsCount = value;
                OnPropertyChanged(nameof(ApprovedClaimsCount));
            }
        }

        public int RejectedClaimsCount
        {
            get => _rejectedClaimsCount;
            set
            {
                _rejectedClaimsCount = value;
                OnPropertyChanged(nameof(RejectedClaimsCount));
            }
        }

        // Commands for handling button clicks
        public ICommand ViewPendingDetailsCommand { get; }
        public ICommand ViewApprovedDetailsCommand { get; }
        public ICommand ViewRejectedDetailsCommand { get; }

        public DashboardViewModel()
        {
            // Initialize counts
            PendingClaimsCount = 10; // Replace with actual data retrieval logic
            ApprovedClaimsCount = 5;  // Replace with actual data retrieval logic
            RejectedClaimsCount = 3;   // Replace with actual data retrieval logic

            // Set up commands
            ViewPendingDetailsCommand = new RelayCommand(ViewPendingDetails);
            ViewApprovedDetailsCommand = new RelayCommand(ViewApprovedDetails);
            ViewRejectedDetailsCommand = new RelayCommand(ViewRejectedDetails);
        }

        // Command methods
        private void ViewPendingDetails(object parameter)
        {
            // Logic for viewing pending claims details
            // You can implement navigation or other actions here
            MessageBox.Show("Viewing Pending Claims Details");
        }

        private void ViewApprovedDetails(object parameter)
        {
            // Logic for viewing approved claims details
            // You can implement navigation or other actions here
            MessageBox.Show("Viewing Approved Claims Details");
        }

        private void ViewRejectedDetails(object parameter)
        {
            // Logic for viewing rejected claims details
            // You can implement navigation or other actions here
            MessageBox.Show("Viewing Rejected Claims Details");
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
