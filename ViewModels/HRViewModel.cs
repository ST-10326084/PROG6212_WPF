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
        public ICommand LoadClaimsCommand { get; }

        public ObservableCollection<Claim> PendingClaims { get; set; }

        // Updated HRViewModel constructor
        public HRViewModel()
        {
            // Initialize commands
            GenerateReportCommand = new RelayCommand(GenerateReport);
            LoadClaimsCommand = new RelayCommand(_ => LoadPendingClaims());

            // Initialize PendingClaims
            PendingClaims = new ObservableCollection<Claim>();

            // Load claims from file at startup
            LoadPendingClaims();
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
                        if (parts.Length == 6) // Ensure it has 6 parts: ID, HoursWorked, HourlyRate, AdditionalNotes, DocumentPath, Status
                        {
                            var newClaim = new Claim
                            {
                                ClaimId = int.Parse(parts[0]),
                                HoursWorked = int.Parse(parts[1]),
                                HourlyRate = decimal.Parse(parts[2]),
                                AdditionalNotes = parts[3],
                                _documentPath = parts[4],
                                Status = string.IsNullOrWhiteSpace(parts[5]) ? "Pending" : parts[5].Trim(),
                            };
                            PendingClaims.Add(newClaim);
                        }
                    }
                }

                StatusMessage = "Claims loaded successfully.";
            }
            else
            {
                StatusMessage = "Claim data file not found.";
            }
        }

        private void GenerateReport(object obj)
        {
            try
            {
                string reportFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ClaimReport.csv");

                var filteredClaims = PendingClaims.Where(c => c.HoursWorked >= MinHours && c.HoursWorked <= MaxHours);

                using (var writer = new StreamWriter(reportFile))
                {
                    writer.WriteLine("ClaimId,HoursWorked,HourlyRate,AdditionalNotes,DocumentPath,Status");
                    foreach (var claim in filteredClaims)
                    {
                        writer.WriteLine($"{claim.ClaimId},{claim.HoursWorked},{claim.HourlyRate},{claim.AdditionalNotes},{claim._documentPath},{claim.Status}");
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
