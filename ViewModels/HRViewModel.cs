using PROG6212_WPF.Commands;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace PROG6212_WPF.ViewModels
{
    internal class HRViewModel : BaseViewModel
    {
        private int _minHours;
        public int MinHours
        {
            get => _minHours;
            set
            {
                _minHours = value;
                OnPropertyChanged(nameof(MinHours));
            }
        }

        private int _maxHours;
        public int MaxHours
        {
            get => _maxHours;
            set
            {
                _maxHours = value;
                OnPropertyChanged(nameof(MaxHours));
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

        public ICommand GenerateReportCommand { get; }

        public ObservableCollection<Claim> PendingClaims { get; set; }

        public HRViewModel()
        {
            // Initialize command
            GenerateReportCommand = new RelayCommand(GenerateReport);

            // Load data (replace with actual data retrieval logic)
            PendingClaims = LoadPendingClaims();
        }

        private ObservableCollection<Claim> LoadPendingClaims()
        {
            // Simulated data loading
            return new ObservableCollection<Claim>
            {
                new Claim { ClaimId = 1, HoursWorked = 35, HourlyRate = 50, AdditionalNotes = "Valid Claim", Status = "Pending" },
                new Claim { ClaimId = 2, HoursWorked = 42, HourlyRate = 55, AdditionalNotes = "Over hours", Status = "Pending" },
            };
        }

        private void GenerateReport(object obj)
        {
            try
            {
                string reportFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ClaimReport.csv");

                var filteredClaims = PendingClaims.Where(c => c.HoursWorked >= MinHours && c.HoursWorked <= MaxHours);

                using (var writer = new StreamWriter(reportFile))
                {
                    writer.WriteLine("ClaimId,HoursWorked,HourlyRate,AdditionalNotes,Status");
                    foreach (var claim in filteredClaims)
                    {
                        writer.WriteLine($"{claim.ClaimId},{claim.HoursWorked},{claim.HourlyRate},{claim.AdditionalNotes},{claim.Status}");
                    }
                }

                StatusMessage = $"Report generated successfully at: {reportFile}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
            }
        }
    }
}
