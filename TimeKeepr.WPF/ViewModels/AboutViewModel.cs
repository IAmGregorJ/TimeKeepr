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

using System.Windows.Input;
using TimeKeepr.WPF.Helper;

namespace TimeKeepr.WPF.ViewModels
{
    internal class AboutViewModel : BaseViewModel
    {
        public ICommand LinkCommand
        {
            get
            {
                return new BaseCommand(ClickLink);
            }
        }
        private void ClickLink()
        {
            var destinationurl = "https://gregorj.org";
            var sInfo = new System.Diagnostics.ProcessStartInfo(destinationurl)
            {
                UseShellExecute = true,
            };
            System.Diagnostics.Process.Start(sInfo);
        }

        public ICommand MailCommand
        {
            get
            {
                return new BaseCommand(ClickMail);
            }
        }
        private void ClickMail()
        {
            var destinationurl = "mailto:IAm@GregorJ.org?subject=Timekeepr";
            var sInfo = new System.Diagnostics.ProcessStartInfo(destinationurl)
            {
                UseShellExecute = true,
            };
            System.Diagnostics.Process.Start(sInfo);
        }

        public ICommand ManualCommand
        {
            get
            {
                return new BaseCommand(ClickManual);
            }
        }
        private void ClickManual()
        {
            var destinationurl = "https://gregorj.org/TimeKeepr/Manuals/TimeKeepr.Help.pdf";
            var sInfo = new System.Diagnostics.ProcessStartInfo(destinationurl)
            {
                UseShellExecute = true,
            };
            System.Diagnostics.Process.Start(sInfo);
        }

        public ICommand SourceCommand
        {
            get
            {
                return new BaseCommand(ClickSource);
            }
        }
        private void ClickSource()
        {
            var destinationurl = "https://github.com/IAmGregorJ/TimeKeepr";
            var sInfo = new System.Diagnostics.ProcessStartInfo(destinationurl)
            {
                UseShellExecute = true,
            };
            System.Diagnostics.Process.Start(sInfo);
        }

    }
}