using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using PROG6212_WPF.Commands;

namespace PROG6212_WPF.ViewModels
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        public string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dashboard_data.txt");
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
            // Load claim counts from the file
            LoadDataFromFile(filePath);
        }

        private void LoadDataFromFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    var lines = File.ReadAllLines(filePath);

                    // Reset counts
                    PendingClaimsCount = 0;
                    ApprovedClaimsCount = 0;
                    RejectedClaimsCount = 0;

                    // Iterate through each line in the file
                    foreach (var line in lines)
                    {
                        if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                            continue; // Skip empty or comment lines

                        var parts = line.Split(',');
                        if (parts.Length >= 5) // Ensure there are enough parts
                        {
                            var status = parts[4].Trim(); // Get the status part
                            switch (status)
                            {
                                case "":
                                    PendingClaimsCount++;
                                    break;
                                case "Pending":
                                    PendingClaimsCount++;
                                    break;
                                case "Approved":
                                    ApprovedClaimsCount++;
                                    break;
                                case "Rejected":
                                    RejectedClaimsCount++;
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    // Set default values when the file is not found
                    MessageBox.Show("Claims data file not found. Default values will be used.");
                    PendingClaimsCount = 0;
                    ApprovedClaimsCount = 0;
                    RejectedClaimsCount = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data from file: {ex.Message}");
                // Set default values in case of an error
                PendingClaimsCount = 0;
                ApprovedClaimsCount = 0;
                RejectedClaimsCount = 0;
            }
        }

        // Command methods
        private void ViewPendingDetails(object parameter)
        {
            MessageBox.Show("Viewing Pending Claims Details");
        }

        private void ViewApprovedDetails(object parameter)
        {
            MessageBox.Show("Viewing Approved Claims Details");
        }

        private void ViewRejectedDetails(object parameter)
        {
            MessageBox.Show("Viewing Rejected Claims Details");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
