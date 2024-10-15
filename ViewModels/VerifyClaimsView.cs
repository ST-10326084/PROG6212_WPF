using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using PROG6212_WPF.Commands;

namespace PROG6212_WPF.ViewModels
{
    public class Claim
    {
        public int ClaimId { get; set; }
        public string Contractor { get; set; }
        public string Project { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public DateTime SubmissionDate { get; set; }
    }

    public class VerifyClaimsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Claim> PendingClaims { get; set; }
        private Claim _selectedClaim;

        public Claim SelectedClaim
        {
            get => _selectedClaim;
            set
            {
                _selectedClaim = value;
                OnPropertyChanged(nameof(SelectedClaim));
            }
        }

        public ICommand ApproveClaimCommand { get; }
        public ICommand RejectClaimCommand { get; }

        public VerifyClaimsViewModel()
        {
            PendingClaims = new ObservableCollection<Claim>
            {
                new Claim { ClaimId = 1, Contractor = "Contractor A", Project = "Project X", Amount = 1000, Status = "Pending", SubmissionDate = DateTime.Now },
                new Claim { ClaimId = 2, Contractor = "Contractor B", Project = "Project Y", Amount = 1500, Status = "Pending", SubmissionDate = DateTime.Now }
                // Add more claims as needed
            };

            ApproveClaimCommand = new RelayCommand(ApproveClaim, CanApproveOrReject);
            RejectClaimCommand = new RelayCommand(RejectClaim, CanApproveOrReject);
        }

        private void ApproveClaim(object parameter)
        {
            if (SelectedClaim != null)
            {
                SelectedClaim.Status = "Approved";
                OnPropertyChanged(nameof(PendingClaims));
            }
        }

        private void RejectClaim(object parameter)
        {
            if (SelectedClaim != null)
            {
                SelectedClaim.Status = "Rejected";
                OnPropertyChanged(nameof(PendingClaims));
            }
        }

        private bool CanApproveOrReject(object parameter)
        {
            return SelectedClaim != null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
