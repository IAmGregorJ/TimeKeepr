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
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using TimeKeepr.Domain.Models;
using TimeKeepr.EntityFramework;
using TimeKeepr.EntityFramework.Services;
using TimeKeepr.WPF.Globals;

namespace TimeKeepr.WPF.ViewModels
{
    public class StatisticsViewModel : BaseViewModel
    {
        #region lists properties

        private List<Happening> _workHoursWeek;
        public List<Happening> WorkHoursWeek
        {
            get => _workHoursWeek;
            set
            {
                _workHoursWeek = value;
                OnPropertyChanged(() => WorkHoursWeek);
            }
        }

        private List<Happening> _hoursInMeeting;
        public List<Happening> HoursInMeeting
        {
            get => _hoursInMeeting;
            set
            {
                _hoursInMeeting = value;
                OnPropertyChanged(() => HoursInMeeting);
            }
        }

        private List<Happening> _categoryHoursWeek;
        public List<Happening> CategoryHoursWeek
        {
            get => _categoryHoursWeek;
            set
            {
                _categoryHoursWeek = value;
                OnPropertyChanged(() => CategoryHoursWeek);
            }
        }

        #endregion lists properties

        #region Happening properties
        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(() => Id);
            }
        }

        private string _category;
        public string Category
        {
            get => _category;
            set
            {
                _category = value;
                OnPropertyChanged(() => Category);
            }
        }

        private bool _isMeeting;
        public bool IsMeeting
        {
            get => _isMeeting;
            set
            {
                _isMeeting = value;
                OnPropertyChanged(() => IsMeeting);
            }
        }

        private double _timeInHours;
        public double TimeInHours
        {
            get => _timeInHours;
            set
            {
                _timeInHours = value;
                OnPropertyChanged(() => TimeInHours);
            }
        }

        private double _isMeetingHours;
        public double IsMeetingHours
        {
            get => _isMeetingHours;
            set
            {
                _isMeetingHours = value;
                OnPropertyChanged(() => IsMeetingHours);
            }
        }

        private DateTime _eventDate;
        public DateTime EventDate
        {
            get => _eventDate;
            set
            {
                _eventDate = value;
                OnPropertyChanged(() => EventDate);
            }
        }

        private int _year;
        public int Year
        {
            get => _year;
            set
            {
                _year = value;
                OnPropertyChanged(() => Year);
            }
        }

        private int _weekNr;
        public int WeekNr
        {
            get => _weekNr;
            set
            {
                _weekNr = value;
                OnPropertyChanged(() => WeekNr);
            }
        }

        #endregion Happening properties

        #region userinfo properties
        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged(() => FirstName);
            }
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged(() => LastName);
            }
        }

        private string _fullName = string.Empty;
        public string FullName
        {
            get => _fullName;
            set
            {
                _fullName = value;
                OnPropertyChanged(() => FullName);
            }
        }

        private string _workPlace = string.Empty;
        public string WorkPlace
        {
            get => _workPlace;
            set
            {
                _workPlace = value;
                OnPropertyChanged(() => WorkPlace);
            }
        }

        private double _hoursPerWeek;
        public double HoursPerWeek
        {
            get => _hoursPerWeek;
            set
            {
                _hoursPerWeek = value;
                OnPropertyChanged(() => HoursPerWeek);
            }
        }

        private double? _previousSaldo = 0;
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

        private string _saldo = string.Empty;
        public string Saldo
        {
            get => _saldo;
            set
            {
                _saldo = value;
                OnPropertyChanged(() => Saldo);
            }
        }

        #endregion userinfo properties

        //TOTO Find a way to export the data (Lists, grids, whatever) to pdf and mail them... or MAYBE a central database and admin account?

        //constructor
        public StatisticsViewModel()
        {
            GetCategories();
        }

        private async void GetCategories()
        {
            var serviceUser = new DataService<User>(new TimeKeeprDbContextFactory());
            User user = await serviceUser.GetByUserName(MyGlobals.userLoggedIn);
            FirstName = user.FirstName;
            LastName = user.LastName;
            FullName = FirstName + " " + LastName;
            WorkPlace = user.WorkPlace;
            HoursPerWeek = user.HoursPerWeek;
            PreviousSaldo = user.PreviousSaldo;

            //I still struggle with this syntax - hoping future projects will be more streamlined
            var service = new DataService<Happening>(new TimeKeeprDbContextFactory());
            var UngroupedList = (List<Happening>)await service.GetAll();

            CategoryHoursWeek = UngroupedList
                .Where(x => x.UserName.Contains(MyGlobals.userLoggedIn) && !x.Category.Contains("WorkDay"))
                .GroupBy(a => (a.Category, a.Year, a.WeekNr))
                .Select(c => new Happening
                {
                    Category = c.Key.Category,
                    Year = c.Key.Year,
                    WeekNr = c.Key.WeekNr,
                    IsMeeting = c.Any(c => c.IsMeeting),
                    TimeInHours = c.Sum(c => c.TimeInHours),
                }).ToList();

            WorkHoursWeek = UngroupedList
                .Where(x => x.UserName.Contains(MyGlobals.userLoggedIn) && x.Category.Contains("WorkDay") && x.Year <= DateTime.Now.Year && x.WeekNr <= TimeHelper.TimeHelper.GetIso8601WeekOfYear(DateTime.Now))
                .GroupBy(a => (a.Category, a.Year, a.WeekNr))
                .Select(c => new Happening
                {
                    Category = c.Key.Category,
                    Year = c.Key.Year,
                    WeekNr = c.Key.WeekNr,
                    TimeInHours = c.Sum(c => c.TimeInHours)
                }).ToList();

            HoursInMeeting = UngroupedList
                .Where(x => x.UserName.Contains(MyGlobals.userLoggedIn) && !x.Category.Contains("WorkDay"))
                .GroupBy(a => a.Category)
                .Select(c => new Happening
                {
                    Category = c.Key,
                    TimeInHours = c.Sum(c => c.TimeInHours),
                    IsMeetingHours = c.Sum(c => c.IsMeetingHours)
                }).ToList();

            //HoursInMeeting = UngroupedList
            //    .Where(x => x.UserName.Contains(MyGlobals.userLoggedIn) && !x.Category.Contains("WorkDay") && x.IsMeeting == true)
            //    .GroupBy(a => (a.Category, a.Year, a.WeekNr))
            //    .Select(c => new Happening
            //    {
            //        Category = c.Key.Category,
            //        Year = c.Key.Year,
            //        WeekNr = c.Key.WeekNr,
            //        TimeInHours = c.Sum(c => c.TimeInHours)
            //    }).ToList();

            //If I ever need to filter something like Saldo a bit more... percentage spent in meetings
            //##############################################################
            var TimeSpentInMeetingsThisMonth = UngroupedList
                .Where(x => x.UserName.Contains(MyGlobals.userLoggedIn) && !x.Category.Contains("WorkDay"))
                .Where(a => a.IsMeeting)
                .Where(a => a.EventDate.Month == DateTime.Now.Month)
                .Sum(a => a.TimeInHours);

            var TimeSpentOnProjectsThisMonth = UngroupedList
                .Where(x => x.UserName.Contains(MyGlobals.userLoggedIn) && !x.Category.Contains("WorkDay"))
                .Where(a => a.EventDate.Month == DateTime.Now.Month)
                .Sum(a => a.TimeInHours);

            var PercentSpentInMeetingsThisMonth = ((TimeSpentInMeetingsThisMonth / TimeSpentOnProjectsThisMonth) * 100).ToString("P");
            //##############################################################

            var overTime = (WorkHoursWeek.Sum(item => item.TimeInHours) - (HoursPerWeek * WorkHoursWeek.Count));
            var overTimeT = overTime + PreviousSaldo;
            var overTimeTotal = Math.Round(Convert.ToDecimal(overTimeT), 2);

            Saldo = Convert.ToString(overTimeTotal);

            //Saldo = (PreviousSaldo + (WorkHoursWeek.Sum(item => item.TimeInHours) - 
            //    (HoursPerWeek * WorkHoursWeek.Count))) + " hours";
        }
    }
}