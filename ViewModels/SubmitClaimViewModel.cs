using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using PROG6212_WPF.Commands;

namespace PROG6212_WPF.ViewModels
{
    public class SubmitClaimViewModel : INotifyPropertyChanged
    {
        private int _hoursWorked;
        private decimal _hourlyRate;
        private string _additionalNotes;

        public int HoursWorked
        {
            get => _hoursWorked;
            set
            {
                _hoursWorked = value;
                OnPropertyChanged(nameof(HoursWorked));
            }
        }

        public decimal HourlyRate
        {
            get => _hourlyRate;
            set
            {
                _hourlyRate = value;
                OnPropertyChanged(nameof(HourlyRate));
            }
        }

        public string AdditionalNotes
        {
            get => _additionalNotes;
            set
            {
                _additionalNotes = value;
                OnPropertyChanged(nameof(AdditionalNotes));
            }
        }

        public ICommand SubmitClaimCommand { get; }

        public SubmitClaimViewModel()
        {
            SubmitClaimCommand = new RelayCommand(SubmitClaim);
        }

        private void SubmitClaim(object parameter)
        {
            // Logic to submit the claim
            decimal totalAmount = HoursWorked * HourlyRate;
            MessageBox.Show($"Claim submitted with Total Amount: {totalAmount}. Notes: {AdditionalNotes}");

            // Save claim to the text file
            string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dashboard_data.txt");
            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.WriteLine($"{HoursWorked},{HourlyRate},{AdditionalNotes}");
            }

            // Update the pending claims count
            UpdatePendingClaimsCount();

            // Optionally, reset the fields after submission
            HoursWorked = 0;
            HourlyRate = 0;
            AdditionalNotes = string.Empty;
        }



        private void UpdatePendingClaimsCount()
        {
            string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dashboard_data.txt");

            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath).ToList();
                int pendingClaimsCount = 0;
                int approvedClaimsCount = 0;
                int rejectedClaimsCount = 0;

                // Read existing counts
                foreach (var line in lines)
                {
                    if (line.StartsWith("PendingClaims:"))
                    {
                        int.TryParse(line.Split(':')[1].Trim(), out pendingClaimsCount);
                    }
                    else if (line.StartsWith("ApprovedClaims:"))
                    {
                        int.TryParse(line.Split(':')[1].Trim(), out approvedClaimsCount);
                    }
                    else if (line.StartsWith("RejectedClaims:"))
                    {
                        int.TryParse(line.Split(':')[1].Trim(), out rejectedClaimsCount);
                    }
                }

                // Increment pending claims count for each new submission
                pendingClaimsCount++;

                // Prepare new lines to write back to the file
                lines[lines.IndexOf(lines.First(line => line.StartsWith("PendingClaims:")))] = $"PendingClaims: {pendingClaimsCount}";

                // Write updated counts and existing claims back to the file
                File.WriteAllLines(filePath, lines);
            }
            else
            {
                // If the file does not exist, create it with default counts
                File.WriteAllLines(filePath, new string[]
                {
                    "PendingClaims: 1",
                    "ApprovedClaims: 0",
                    "RejectedClaims: 0",
                    "", // Empty line for separation
                });
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
