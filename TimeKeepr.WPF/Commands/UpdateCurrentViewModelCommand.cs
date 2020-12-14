using System;
using System.Windows.Input;
using TimeKeepr.WPF.State.Navigators;
using TimeKeepr.WPF.ViewModels;

namespace TimeKeepr.WPF.Commands
{
    public class UpdateCurrentViewModelCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private INavigator _navigator;

        public UpdateCurrentViewModelCommand(INavigator navigator)
        {
            _navigator = navigator;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter is ViewType)
            {
                ViewType viewType = (ViewType)parameter;
                switch (viewType)
                {
                    case ViewType.Home:
                        _navigator.CurrentViewModel = new HomeViewModel();
                        break;

                    case ViewType.Categories:
                        _navigator.CurrentViewModel = new CategoriesViewModel();
                        break;

                    case ViewType.Statistics:
                        _navigator.CurrentViewModel = new StatisticsViewModel();
                        break;

                    case ViewType.Help:
                        _navigator.CurrentViewModel = new HelpPageViewModel();
                        break;

                    default:
                        break;
                }
            }
        }
    }
}