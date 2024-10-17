using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using PROG6212_WPF.Commands;
using System.IO;

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
                CommandManager.InvalidateRequerySuggested(); // Notify buttons to check if they can execute
            }
        }

        public ICommand ApproveClaimCommand { get; }
        public ICommand RejectClaimCommand { get; }

        public VerifyClaimsViewModel()
        {
            PendingClaims = new ObservableCollection<Claim>();
            LoadPendingClaims(); // Load claims from the text file

            ApproveClaimCommand = new RelayCommand(ApproveClaim, CanApproveOrReject);
            RejectClaimCommand = new RelayCommand(RejectClaim, CanApproveOrReject);
        }

        private void LoadPendingClaims()
        {
            string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dashboard_data.txt");

            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    if (line.StartsWith("#") || string.IsNullOrWhiteSpace(line) || line.StartsWith("PendingClaims:") || line.StartsWith("ApprovedClaims:") || line.StartsWith("RejectedClaims:"))
                        continue;

                    var parts = line.Split(',');
                    if (parts.Length == 3) // Updated to check for 3 parts
                    {
                        if (decimal.TryParse(parts[0], out decimal hoursWorked) && decimal.TryParse(parts[1], out decimal hourlyRate))
                        {
                            var newClaim = new Claim
                            {
                                ClaimId = PendingClaims.Count + 1,
                                Contractor = "yes", // parts[2], // Get Contractor from input
                                Project = "yes",      // parts[3],    // Get Project from input
                                Amount = hoursWorked * hourlyRate,
                                Status = "Pending",
                                SubmissionDate = DateTime.Now
                            };
                            PendingClaims.Add(newClaim);
                        }
                        else
                        {
                            Console.WriteLine($"Invalid claim data: {line}");
                        }
                    }
                }
            }
        }

        private void ApproveClaim(object parameter)
        {
            if (SelectedClaim != null)
            {
                SelectedClaim.Status = "Approved";
                UpdateDashboardCounts(1, 0); // Update approved claims count
                OnPropertyChanged(nameof(PendingClaims));
            }
        }

        private void RejectClaim(object parameter)
        {
            if (SelectedClaim != null)
            {
                SelectedClaim.Status = "Rejected";
                UpdateDashboardCounts(0, 1); // Update rejected claims count
                OnPropertyChanged(nameof(PendingClaims));
            }
        }

        private void UpdateDashboardCounts(int approvedIncrement, int rejectedIncrement)
        {
            string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dashboard_data.txt");
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath).ToList();
                for (int i = 0; i < lines.Count; i++)
                {
                    if (lines[i].StartsWith("ApprovedClaims:"))
                    {
                        int currentCount = int.Parse(lines[i].Split(':')[1].Trim());
                        lines[i] = $"ApprovedClaims: {currentCount + approvedIncrement}";
                    }
                    else if (lines[i].StartsWith("RejectedClaims:"))
                    {
                        int currentCount = int.Parse(lines[i].Split(':')[1].Trim());
                        lines[i] = $"RejectedClaims: {currentCount + rejectedIncrement}";
                    }
                }
                File.WriteAllLines(filePath, lines);
            }
        }

        private bool CanApproveOrReject(object parameter)
        {
            return SelectedClaim != null; // Returns true only if a claim is selected
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
