using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TimeKeepr.WPF.Components
{
    /// <summary>
    /// Interaction logic for BindablePasswordBox.xaml
    /// </summary>

    /*This usercontrol was intended as an implementation of passwordbox binding - since binding needs a DependencyProperty
     * However, implementing this "broke" the PassWord string in my viewmodel (BaseCommand nullified the string), and this really
     * doesn't do much good anyway, as a SecureString would be the better way to do this. This code MAY be used another time though, 
     * so I'm keeping it within my solution, even though it is never called*/
    public partial class BindablePasswordBox : UserControl
    {
        private bool _isPasswordChanging;


        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(BindablePasswordBox),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    PasswordPropertyChanged, null, false, UpdateSourceTrigger.PropertyChanged));


        private static void PasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BindablePasswordBox passwordBox)
            {
                passwordBox.UpdatePassword();
            }
        }


        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }


        public BindablePasswordBox()
        {
            InitializeComponent();
        }


        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _isPasswordChanging = true;
            Password = passwordBox.Password;
            _isPasswordChanging = false;
        }


        private void UpdatePassword()
        {
            if (!_isPasswordChanging)
            {
                passwordBox.Password = Password;
            }
        }
    }
}