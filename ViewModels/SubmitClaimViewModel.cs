using System.ComponentModel;
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

            // Optionally, reset the fields after submission
            HoursWorked = 0;
            HourlyRate = 0;
            AdditionalNotes = string.Empty;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
