using PROG6212_WPF.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

public class Claim1
{
    public int ClaimId { get; set; }
    public int HoursWorked { get; set; }
    public decimal HourlyRate { get; set; }
    public string AdditionalNotes { get; set; }
    public string Status { get; set; }
}

public class testC : INotifyPropertyChanged
{
    public ObservableCollection<Claim1> PendingClaims { get; set; }
    private Claim1 _selectedClaim;

    public Claim1 SelectedClaim
    {
        get => _selectedClaim;
        set
        {
            _selectedClaim = value;
            OnPropertyChanged(nameof(SelectedClaim));
            CommandManager.InvalidateRequerySuggested();
            System.Diagnostics.Debug.WriteLine($"SelectedClaim changed: {_selectedClaim?.ClaimId}");

            // Display all relevant information for the selected claim
            System.Diagnostics.Debug.WriteLine($"SelectedClaim changed: ID={_selectedClaim?.ClaimId}, Hours={_selectedClaim?.HoursWorked}, Rate={_selectedClaim?.HourlyRate}, Notes={_selectedClaim?.AdditionalNotes}, Status={_selectedClaim?.Status}");

            // Update button states
            ((RelayCommand)ApproveClaimCommand).RaiseCanExecuteChanged();
            ((RelayCommand)RejectClaimCommand).RaiseCanExecuteChanged();
        }
    }

    public ICommand ApproveClaimCommand { get; }
    public ICommand RejectClaimCommand { get; }

    public testC()
    {
        PendingClaims = new ObservableCollection<Claim1>();
        LoadPendingClaims(); // Load claims with IDs and statuses

        ApproveClaimCommand = new RelayCommand(ApproveClaim, CanApproveOrReject);
        RejectClaimCommand = new RelayCommand(RejectClaim, CanApproveOrReject);
    }

    private void LoadPendingClaims()
    {
        // Load hard-coded claims based on the given format
        PendingClaims.Add(new Claim1 { ClaimId = 1, HoursWorked = 12, HourlyRate = 12, AdditionalNotes = "no additional notes", Status = "Approved" });
        PendingClaims.Add(new Claim1 { ClaimId = 2, HoursWorked = 10, HourlyRate = 100, AdditionalNotes = "yes additional notes!", Status = "Pending" });
        PendingClaims.Add(new Claim1 { ClaimId = 3, HoursWorked = 10, HourlyRate = 12, AdditionalNotes = "worked 2 hrs overtime", Status = "" });
        PendingClaims.Add(new Claim1 { ClaimId = 4, HoursWorked = 12, HourlyRate = 8, AdditionalNotes = "Worked 4 hrs overtime", Status = "" });
        PendingClaims.Add(new Claim1 { ClaimId = 5, HoursWorked = 40, HourlyRate = 100, AdditionalNotes = "Weekly pay", Status = "" });
        PendingClaims.Add(new Claim1 { ClaimId = 6, HoursWorked = 1, HourlyRate = 7, AdditionalNotes = "1 hour worked", Status = "" });
        PendingClaims.Add(new Claim1 { ClaimId = 7, HoursWorked = 12, HourlyRate = 123, AdditionalNotes = "no notes today", Status = "" });
    }

    private void ApproveClaim(object parameter)
    {
        if (SelectedClaim != null)
        {
            SelectedClaim.Status = "Approved";
            System.Diagnostics.Debug.WriteLine($"Claim {SelectedClaim.ClaimId} approved.");
            ((RelayCommand)ApproveClaimCommand).RaiseCanExecuteChanged();
            ((RelayCommand)RejectClaimCommand).RaiseCanExecuteChanged();
        }
    }

    private void RejectClaim(object parameter)
    {
        if (SelectedClaim != null)
        {
            SelectedClaim.Status = "Rejected";
            System.Diagnostics.Debug.WriteLine($"Claim {SelectedClaim.ClaimId} rejected.");
            ((RelayCommand)ApproveClaimCommand).RaiseCanExecuteChanged();
            ((RelayCommand)RejectClaimCommand).RaiseCanExecuteChanged();
        }
    }

    private bool CanApproveOrReject(object parameter) => SelectedClaim != null;

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
