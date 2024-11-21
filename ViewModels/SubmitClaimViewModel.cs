using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32; // For file dialog
using PROG6212_WPF.Commands;

namespace PROG6212_WPF.ViewModels
{
    public class SubmitClaimViewModel : INotifyPropertyChanged
    {
        private int _hoursWorked;
        private string _additionalNotes;
        private string _documentPath;
        private decimal _selectedHourlyRate;

        // For unit testing
        public decimal TotalAmount { get; private set; }

        public int HoursWorked
        {
            get => _hoursWorked;
            set
            {
                _hoursWorked = value;
                OnPropertyChanged(nameof(HoursWorked));
            }
        }

        public decimal SelectedHourlyRate
        {
            get => _selectedHourlyRate;
            set
            {
                _selectedHourlyRate = value;
                OnPropertyChanged(nameof(SelectedHourlyRate));
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

        public ObservableCollection<decimal> HourlyRateOptions { get; } = new ObservableCollection<decimal>
        {
            20m, 40m, 50m, 75m, 100m, 125m
        };

        public ICommand SubmitClaimCommand { get; }
        public ICommand UploadDocumentCommand { get; }

        public SubmitClaimViewModel()
        {
            SubmitClaimCommand = new RelayCommand(SubmitClaim);
            UploadDocumentCommand = new RelayCommand(UploadDocument);
        }

        public void SubmitClaim(object parameter)
        {
            if (HoursWorked <= 0 || SelectedHourlyRate <= 0)
            {
                MessageBox.Show("Please ensure all fields are correctly filled before submitting.");
                return;
            }

            // Calculate total amount
            TotalAmount = HoursWorked * SelectedHourlyRate;

            MessageBox.Show($"Claim submitted with Total Amount: {TotalAmount:C}. Notes: {AdditionalNotes}");

            // Save claim to the text file with status as "Pending"
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dashboard_data.txt");
            int newId = GetNextClaimId(filePath);

            // Append new claim data to the file
            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.WriteLine($"{newId},{HoursWorked},{SelectedHourlyRate},{AdditionalNotes},{DocumentPath},Pending");
            }

            // Reset fields after submission
            HoursWorked = 0;
            SelectedHourlyRate = 0;
            AdditionalNotes = string.Empty;
            DocumentPath = string.Empty;
        }

        public int GetNextClaimId(string filePath)
        {
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                var lastLine = lines.LastOrDefault();
                var lastId = lastLine?.Split(',')[0];

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
