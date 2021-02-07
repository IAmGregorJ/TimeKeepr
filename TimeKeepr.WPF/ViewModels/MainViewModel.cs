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

using System.Resources;
using System.Windows;
using System.Windows.Input;
using TimeKeepr.WPF.Helper;
using TimeKeepr.WPF.Localizations;

namespace TimeKeepr.WPF.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ICommand CloseCommand
        {
            get
            {
                return new BaseCommand(ClickClose);
            }
        }
        private void ClickClose()
        {
                CloseApplication();
        }

        public ICommand MinimizeCommand
        {
            get
            {
                return new BaseCommand(ClickMin);
            }
        }
        private void ClickMin()
        {
            MinimizeApplication();
        }
    }
}