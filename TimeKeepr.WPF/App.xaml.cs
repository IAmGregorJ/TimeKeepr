using System.Windows;
using TimeKeepr.WPF.ViewModels;
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
            Window window = new LoginView();
            window.DataContext = new LoginViewModel();
            window.ResizeMode = ResizeMode.NoResize;
            window.ShowDialog();
            InitializeComponent();

            base.OnStartup(e);
        }
    }
}