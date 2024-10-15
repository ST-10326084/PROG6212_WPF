using PROG6212_WPF.ViewModels;
using System.ComponentModel;
using System.Windows.Input;
using PROG6212_WPF.Commands;

namespace PROG6212_WPF.ViewModels
{

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public ICommand NavigateCommand { get; }
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
            CurrentView = new DashboardViewModel(); // Set default view
        }

        private void Navigate(object parameter)
        {
            switch (parameter.ToString())
            {
                case "DashboardView":
                    CurrentView = new DashboardViewModel();
                    break;
                case "SubmitClaimView":
                    CurrentView = new SubmitClaimViewModel();
                    break;
                case "VerifyClaimsView":
                    CurrentView = new VerifyClaimsViewModel();
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}

