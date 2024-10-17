using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using PROG6212_WPF.Commands;

namespace PROG6212_WPF.ViewModels
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        public string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dashboard_data.txt");
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
                    foreach (var line in lines)
                    {
                        var parts = line.Split(':');
                        if (parts.Length == 2)
                        {
                            var key = parts[0];
                            if (int.TryParse(parts[1], out int value))
                            {
                                switch (key)
                                {
                                    case "PendingClaims":
                                        PendingClaimsCount = value;
                                        break;
                                    case "ApprovedClaims":
                                        ApprovedClaimsCount = value;
                                        break;
                                    case "RejectedClaims":
                                        RejectedClaimsCount = value;
                                        break;
                                }
                            }
                            else
                            {
                                MessageBox.Show($"Invalid data format for {key}: {parts[1]}");
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
