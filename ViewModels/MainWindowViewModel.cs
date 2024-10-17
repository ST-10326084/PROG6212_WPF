using System.ComponentModel;
using System.Windows.Input;
using PROG6212_WPF.Commands;
using PROG6212_WPF.Views;

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
            CurrentView = new DashboardView(); // Set default view
        }

        private void Navigate(object parameter)
        {
            switch (parameter.ToString())
            {
                case "DashboardView":
                    CurrentView = new DashboardView(); // Instantiate UserControl
                    break;
                case "SubmitClaimView":
                    CurrentView = new SubmitClaimView(); // Instantiate UserControl
                    break;
                case "VerifyClaimsView":
                    CurrentView = new VerifyClaimsView(); // Instantiate UserControl
                    break;             
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }


}

