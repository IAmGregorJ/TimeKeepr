// This file is part of TimeKeepr.
//
// TimeKeepr is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// TimeKeepr is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY - without even the implied warranty of
//
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
// See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with TimeKeepr.  If not, see <https://www.gnu.org/licenses/>.

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
                        _navigator.CurrentViewModel = new LoggingViewModel();
                        break;

                    case ViewType.Categories:
                        _navigator.CurrentViewModel = new CategoriesViewModel();
                        break;

                    case ViewType.Statistics:
                        _navigator.CurrentViewModel = new StatisticsViewModel();
                        break;

                    case ViewType.About:
                        _navigator.CurrentViewModel = new AboutViewModel();
                        break;

                    default:
                        break;
                }
            }
        }
    }
}