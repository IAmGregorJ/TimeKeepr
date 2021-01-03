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
using System.Resources;
using System.Windows.Input;
using GJEncryption;
using TimeKeepr.Domain.Models;
using TimeKeepr.EntityFramework;
using TimeKeepr.EntityFramework.Services;
using TimeKeepr.WPF.Globals;
using TimeKeepr.WPF.Helper;
using TimeKeepr.WPF.Localizations;

namespace TimeKeepr.WPF.ViewModels
{
    internal class LoginViewModel : BaseViewModel
    {
        #region properties used for login
        private string _username;
        public string UserName
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(() => UserName);
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(() => Password);
            }
        }
        #endregion
        #region properties used in registration
        private string _firstname;
        public string FirstName
        {
            get => _firstname;
            set
            {
                _firstname = value;
                OnPropertyChanged(() => FirstName);
            }
        }

        private string _lastname;
        public string LastName
        {
            get => _lastname;
            set
            {
                _lastname = value;
                OnPropertyChanged(() => LastName);
            }
        }

        private string _email;
        public string EMail
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(() => EMail);
            }
        }

        private string _workplace;
        public string WorkPlace
        {
            get => _workplace;
            set
            {
                _workplace = value;
                OnPropertyChanged(() => WorkPlace);
            }
        }

        private double? _hoursperweek;
        public double? HoursPerWeek
        {
            get => _hoursperweek;
            set
            {
                if (_hoursperweek != value)
                {
                    _hoursperweek = value;
                    OnPropertyChanged(() => HoursPerWeek);
                }
            }
        }
        private double? _previousSaldo;
        public double? PreviousSaldo
        {
            get => _previousSaldo;
            set
            {
                if (_previousSaldo != value)
                {
                    _previousSaldo = value;
                    OnPropertyChanged(() => PreviousSaldo);
                }
            }
        }

        #endregion
        //Button disable
        private string _buttonIsEnabled = "true";

        public string ButtonIsEnabled
        {
            get => _buttonIsEnabled;
            set
            {
                _buttonIsEnabled = value;
                OnPropertyChanged(() => ButtonIsEnabled);
            }
        }

        ResourceManager rm = new ResourceManager(typeof(Resources));

        #region login logic
        public ICommand LoginCommand
        {
            get
            {
                return new BaseCommand(ClickLogin);
            }
        }
        private async void ClickLogin()
        {
            ButtonIsEnabled = "false";
            UserName = _username;
            /*For the password, I should REALLY be using a SecureString and also a passwordbox, but using SecureString presents unique problems
             * that are exponentially complicated when using MVVM, and passwordbox does the same. Not to mention that the use of async
             * complicates BOTH of these processes. Maybe one day I'll figure it out, but in the interest of actually FINISHING this project, I'm
             * leaving these things for another day.*/
            Password = _password;  // change this to incorporate hash and verify
            //instantiate DataService
            var service = new DataService<User>(new TimeKeeprDbContextFactory());
            User user = await service.GetByUserName(UserName);
            if (user == null)
            {
                ShowMessageBox(rm.GetString("User_notexist"));
                ButtonIsEnabled = "true";
            }
            else
            {
                byte[] saltByte = Convert.FromBase64String(user.Salt); //salt from database
                var hashNow = PBKDF2.HashPassword(Password, saltByte); //hash of the password from the input
                string newHash = Convert.ToBase64String(hashNow);
                if (user.PasswordHash == newHash)
                {
                    LoggedInCloseDialog(user);
                }
                else
                {
                    ShowMessageBox(rm.GetString("Password_notcorrect"));
                    Environment.Exit(1);
                }
            }
        }
        #endregion
        #region registration logic
        public ICommand CreateCommand
        {
            get
            {
                return new BaseCommand(ClickAddUser);
            }
        }
        private async void ClickAddUser()
        {
            ButtonIsEnabled = "false";
            if (HoursPerWeek == null)
                HoursPerWeek = 0.0;
            if (PreviousSaldo == null)
                PreviousSaldo = 0.0;
            var _salt = PBKDF2.CreateSalt();
            User user = new User()
            {
                EMail = _email,
                UserName = _username,
                PasswordHash = Convert.ToBase64String(PBKDF2.HashPassword(_password, _salt)),
                FirstName = _firstname,
                LastName = _lastname,
                WorkPlace = _workplace,
                HoursPerWeek = (double)_hoursperweek,
                PreviousSaldo = (double)_previousSaldo,
                Salt = Convert.ToBase64String(_salt)
            };
            //instantiate DataService
            var service = new DataService<User>(new TimeKeeprDbContextFactory());
            //call Create<T>
            await service.Create(user);
            LoggedInCloseDialog(user);
        }
        //LoggedInCloseDialog is a VERY clumsy and NOT MVVM way of closing the current (login) window and opening a new one.
        public void LoggedInCloseDialog(User user)
        {
            MyGlobals.userLoggedIn = user.UserName;
            MyGlobals.usersHours = user.HoursPerWeek;

            MainWindow mainWindow = new MainWindow();
            mainWindow.DataContext = new MainViewModel();
            mainWindow.Show();
            App.Current.MainWindow.Close();
            App.Current.MainWindow = mainWindow;
        }
        #endregion
    }
}