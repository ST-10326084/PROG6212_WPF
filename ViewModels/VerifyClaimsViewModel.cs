using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using PROG6212_WPF.Commands;
using PROG6212_WPF.Commands;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32; // Include this for file dialog
using PROG6212_WPF.Commands;


namespace PROG6212_WPF.ViewModels
{
   

    public class Claim
    {
        public int ClaimId { get; set; }
        public int HoursWorked { get; set; }
        public decimal HourlyRate { get; set; }
        public string AdditionalNotes { get; set; }
        public string Status { get; set; }
        public string _documentPath { get; set; }
    }

    public class VerifyClaimsViewModel : INotifyPropertyChanged
    {
        public ICommand AutoVerifyCommand { get; }
        public ICommand ResetClaimsCommand { get; }
        public ObservableCollection<Claim> PendingClaims { get; set; }
        private Claim _selectedClaim;

        public Claim SelectedClaim
        {
            get => _selectedClaim;
            set
            {
                _selectedClaim = value;
                OnPropertyChanged(nameof(SelectedClaim));
                CommandManager.InvalidateRequerySuggested();

                // Display all relevant information for the selected claim
                System.Diagnostics.Debug.WriteLine($"SelectedClaim changed: ID={_selectedClaim?.ClaimId}, Hours={_selectedClaim?.HoursWorked}, Rate={_selectedClaim?.HourlyRate}, Notes={_selectedClaim?.AdditionalNotes}, Status={_selectedClaim?.Status}");

                // Update button states
                ((RelayCommand)ApproveClaimCommand).RaiseCanExecuteChanged();
                ((RelayCommand)RejectClaimCommand).RaiseCanExecuteChanged();
            }
        }

        private string _statusMessage;
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged(nameof(StatusMessage));
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
            AutoVerifyCommand = new RelayCommand(AutoVerifyClaims); // New AutoVerifyCommand
            ResetClaimsCommand = new RelayCommand(ResetAllClaimsToPending); // New ResetClaimsCommand
        }

        private void LoadPendingClaims()
        {
            PendingClaims.Clear(); // Clear the existing claims

            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dashboard_data.txt");

            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                bool isClaimsSection = false; // Track whether we are in the claims section

                foreach (var line in lines)
                {
                    if (line.StartsWith("#")) // Ignore comment lines
                        continue;

                    if (line.StartsWith("ID,")) // Identify the start of the claims section
                    {
                        isClaimsSection = true;
                        continue;
                    }

                    if (isClaimsSection)
                    {
                        if (string.IsNullOrWhiteSpace(line))
                            continue; // Ignore empty lines

                        var parts = line.Split(',');
                        if (parts.Length == 6) // Ensure it has 5 parts: ID, HoursWorked, HourlyRate, AdditionalNotes, Status
                        {
                            var newClaim = new Claim
                            {
                                ClaimId = int.Parse(parts[0]),
                                HoursWorked = int.Parse(parts[1]),
                                HourlyRate = decimal.Parse(parts[2]),
                                AdditionalNotes = parts[3],
                                _documentPath = parts[4],
                                Status = string.IsNullOrWhiteSpace(parts[5]) ? "Pending" : parts[5].Trim(),
                                // Assuming SubmissionDate isn't needed at this point
                            };
                            PendingClaims.Add(newClaim);
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
                System.Diagnostics.Debug.WriteLine($"Claim {SelectedClaim.ClaimId} approved.");
                UpdateClaimStatusInFile(SelectedClaim); // Update the status in the file
                OnPropertyChanged(nameof(PendingClaims));
                LoadPendingClaims();
            }
        }

        private void RejectClaim(object parameter)
        {
            if (SelectedClaim != null)
            {
                SelectedClaim.Status = "Rejected";
                System.Diagnostics.Debug.WriteLine($"Claim {SelectedClaim.ClaimId} rejected.");
                UpdateClaimStatusInFile(SelectedClaim); // Update the status in the file
                OnPropertyChanged(nameof(PendingClaims));
                LoadPendingClaims();
            }
        }

        private void UpdateClaimStatusInFile(Claim claim)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dashboard_data.txt");
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath).ToList();
                for (int i = 0; i < lines.Count; i++)
                {
                    if (lines[i].StartsWith("#") || string.IsNullOrWhiteSpace(lines[i]))
                        continue;

                    var parts = lines[i].Split(',');
                    if (parts.Length >= 6 && parts[0] == claim.ClaimId.ToString()) // Adjusted for 6 parts
                    {
                        lines[i] = $"{parts[0]},{parts[1]},{parts[2]},{parts[3]},{parts[4]},{claim.Status}"; // Update status
                        break;
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

        // Automated setup with a click of a button
        private void AutoVerifyClaims(object parameter)
        {
            foreach (var claim in PendingClaims)
            {
                // Auto-verify logic
                if (claim.HoursWorked <= 40 && claim.HourlyRate <= 50)
                {
                    claim.Status = "Approved";
                }
                else
                {
                    claim.Status = "Denied";
                }

                // Update the status in the file for the current claim
                UpdateClaimStatusInFile(claim);
            }

            StatusMessage = "Auto claim verification complete.";
            OnPropertyChanged(nameof(PendingClaims)); // Refresh the list view to show updated statuses
            LoadPendingClaims();
        }


        private void ResetAllClaimsToPending(object parameter)
        {
            foreach (var claim in PendingClaims)
            {
                claim.Status = "Pending"; // Reset status to "Pending"
            }

            StatusMessage = "All claims have been reset to Pending.";
            OnPropertyChanged(nameof(PendingClaims)); // Refresh the list

            // After resetting all claims, write the updated statuses back to the file
            UpdateToPendingInFile();
            LoadPendingClaims();
        }

        private void UpdateToPendingInFile()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dashboard_data.txt");

            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath).ToList();
                bool isClaimsSection = false;

                for (int i = 0; i < lines.Count; i++)
                {
                    var line = lines[i];
                    if (line.StartsWith("#") || string.IsNullOrWhiteSpace(line))
                        continue;

                    // Identify the start of the claims section
                    if (line.StartsWith("ID,"))
                    {
                        isClaimsSection = true;
                        continue;
                    }

                    if (isClaimsSection)
                    {
                        var parts = line.Split(',');

                        // Update the status of each claim to "Pending" in the file
                        if (parts.Length == 6)
                        {
                            var claim = PendingClaims.FirstOrDefault(c => c.ClaimId.ToString() == parts[0]);
                            if (claim != null)
                            {
                                // Replace the old status with "Pending"
                                lines[i] = $"{claim.ClaimId},{claim.HoursWorked},{claim.HourlyRate},{claim.AdditionalNotes},{claim._documentPath},Pending";
                            }
                        }
                    }
                }

                // Write all updated lines back to the file
                File.WriteAllLines(filePath, lines);
            }
        }


    }
}
