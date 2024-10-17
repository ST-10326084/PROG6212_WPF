using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32; // Include this for file dialog
using PROG6212_WPF.Commands;

namespace PROG6212_WPF.ViewModels
{
    public class SubmitClaimViewModel : INotifyPropertyChanged
    {
        private int _hoursWorked;
        private decimal _hourlyRate;
        private string _additionalNotes;
        private string _documentPath;

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

        public string DocumentPath
        {
            get => _documentPath;
            set
            {
                _documentPath = value;
                OnPropertyChanged(nameof(DocumentPath));
            }
        }

        public ICommand SubmitClaimCommand { get; }
        public ICommand UploadDocumentCommand { get; }

        public SubmitClaimViewModel()
        {
            SubmitClaimCommand = new RelayCommand(SubmitClaim);
            UploadDocumentCommand = new RelayCommand(UploadDocument);
        }

        private void SubmitClaim(object parameter)
        {
            // Logic to submit the claim
            decimal totalAmount = HoursWorked * HourlyRate;
            MessageBox.Show($"Claim submitted with Total Amount: {totalAmount:C}. Notes: {AdditionalNotes}");

            // Save claim to the text file with status as "Pending"
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dashboard_data.txt");
            int newId = GetNextClaimId(filePath); // Get new ID for the claim
            string status = string.IsNullOrEmpty(DocumentPath) ? "Pending" : DocumentPath;

            // Append new claim data to the file
            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.WriteLine($"{newId},{HoursWorked},{HourlyRate},{AdditionalNotes},{status}");
            }

            // Reset fields after submission
            HoursWorked = 0;
            HourlyRate = 0;
            AdditionalNotes = string.Empty;
            DocumentPath = string.Empty; // Reset document path
        }

        private int GetNextClaimId(string filePath)
        {
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                var submittedClaims = lines.SkipWhile(line => !line.StartsWith("# Submitted Claims")).Skip(1);
                var lastId = submittedClaims.LastOrDefault()?.Split(',')[0];

                return lastId == null ? 1 : int.Parse(lastId) + 1;
            }
            return 1; // Return 1 if no file exists
        }

        private void UploadDocument(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Documents|*.pdf;*.doc;*.docx|All Files|*.*" // Set file types
            };
            if (openFileDialog.ShowDialog() == true)
            {
                DocumentPath = openFileDialog.FileName; // Save the path of the selected document
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
