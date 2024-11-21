using System.ComponentModel;
using System.Net.Sockets;
using System.Windows.Input;
using PROG6212_WPF.Commands;
using PROG6212_WPF.Views;

namespace PROG6212_WPF.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public ICommand NavigateCommand { get; }
        public ICommand RoleSelectedCommand { get; }
        private object _currentView;

        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public MainWindowViewModel()
        {
            NavigateCommand = new RelayCommand(Navigate);
            CurrentView = new RoleSelectionView(); // Set default view

            RoleSelectedCommand = new RelayCommand(SelectRole);
        }

        private void Navigate(object parameter)
        {
            switch (parameter.ToString())
            {
                case "RoleSelectionView":
                    CurrentView = new RoleSelectionView();
                    break;
                case "DashboardView":
                    CurrentView = new DashboardView(); // Instantiate UserControl
                    break;
                case "SubmitClaimView":
                    CurrentView = new SubmitClaimView(); // Instantiate UserControl
                    break;
                case "VerifyClaimsView":
                    CurrentView = new VerifyClaimsView(); // Instantiate UserControl
                    break;
                case "HRView":
                    CurrentView = new HRView();
                    break;
                default:
                    throw new ArgumentException("Invalid view name");
            }
        }

        private void SelectRole(object role)
        {
            switch (role.ToString())
            {
                case "Lecturer":
                    CurrentView = new SubmitClaimView(); // Replace with actual Lecturer view
                    break;
                case "AcademicManager":
                    CurrentView = new VerifyClaimsView(); // Replace with actual Academic Manager view
                    break;
                case "HR":
                    CurrentView = new HRView(); // Replace with actual HR view
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }


}

