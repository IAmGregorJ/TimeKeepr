using System.ComponentModel;
using System.Resources;
using System.Windows;
using System.Windows.Input;
using TimeKeepr.WPF.Commands;
using TimeKeepr.WPF.Helper;
using TimeKeepr.WPF.Localizations;
using TimeKeepr.WPF.ViewModels;

namespace TimeKeepr.WPF.State.Navigators
{
    public class Navigator : BaseViewModel, INavigator, INotifyPropertyChanged
    {
        private BaseViewModel _currentViewModel;

        public BaseViewModel CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        public ICommand UpdateCurrentViewModelCommand => new UpdateCurrentViewModelCommand(this);

        public new event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand CloseCommand { get { return new BaseCommand(ClickClose); } }
        private void ClickClose()
        {
            ResourceManager rm = new ResourceManager(typeof(Resources));
            MessageBoxResult messageBoxResult = MessageBox.Show(rm.GetString("Exit"), "Exit confirmation", MessageBoxButton.YesNo);

            if (messageBoxResult == MessageBoxResult.Yes)
                CloseApplication();
        }

        public ICommand MinimizeCommand { get { return new BaseCommand(ClickMin); } }
        private void ClickMin()
        {
            MinimizeApplication();
        }
    }
}