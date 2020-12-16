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
                    TimeInHours = c.Sum(c => c.TimeInHours)
                }).ToList();

            WorkHoursWeek = UngroupedList
                .Where(x => x.UserName.Contains(MyGlobals.userLoggedIn) && x.Category.Contains("WorkDay"))
                .GroupBy(a => (a.Category, a.Year, a.WeekNr))
                .Select(c => new Happening
                {
                    Category = c.Key.Category,
                    Year = c.Key.Year,
                    WeekNr = c.Key.WeekNr,
                    TimeInHours = c.Sum(c => c.TimeInHours)
                }).ToList();

            HoursInMeeting = UngroupedList
                .Where(x => x.UserName.Contains(MyGlobals.userLoggedIn) && !x.Category.Contains("WorkDay") && x.IsMeeting == true)
                .GroupBy(a => (a.Category, a.Year, a.WeekNr))
                .Select(c => new Happening
                {
                    Category = c.Key.Category,
                    Year = c.Key.Year,
                    WeekNr = c.Key.WeekNr,
                    TimeInHours = c.Sum(c => c.TimeInHours)
                }).ToList();

            var SomethingDouble = (WorkHoursWeek
                .Where(item => item.IsMeeting)
                .Sum(item => item.TimeInHours));

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



            Saldo = (WorkHoursWeek.Sum(item => item.TimeInHours) - 
                (HoursPerWeek * WorkHoursWeek.Count)).ToString("F2") + " hours";
        }
    }
}