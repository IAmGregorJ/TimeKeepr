using System.Windows;
using TimeKeepr.WPF.ViewModels;
using TimeKeepr.WPF.Views;
using TimeKeepr.EntityFramework;
using TimeKeepr.EntityFramework.Services;
using TimeKeepr.Domain.Models;
using System.Windows.Markup;
using System.Globalization;

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