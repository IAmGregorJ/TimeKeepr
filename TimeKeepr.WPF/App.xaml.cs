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

using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using TimeKeepr.WPF.Views;

namespace TimeKeepr.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //making sure the decimal character is either , or . depending on locale
            FrameworkElement.LanguageProperty.OverrideMetadata(
            typeof(FrameworkElement),
            new FrameworkPropertyMetadata(
                XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("da-DK");
            Window window = new LoginView();
            window.ResizeMode = ResizeMode.NoResize;
            window.ShowDialog();
            InitializeComponent();
            base.OnStartup(e);
        }
    }
}