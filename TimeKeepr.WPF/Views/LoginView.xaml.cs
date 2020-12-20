using System.Windows;
using System.Windows.Input;

namespace TimeKeepr.WPF.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            Username_txtbox.Focus();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        private void WindowMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

    }
}