using System;
using System.Collections.Generic;
using System.Linq;
using TimeKeepr.Domain.Models;
using TimeKeepr.EntityFramework;
using TimeKeepr.EntityFramework.Services;
using TimeKeepr.WPF.Globals;

namespace TimeKeepr.WPF.ViewModels
{
    public class StatisticsViewModel : BaseViewModel
    {
        #region lists properties
        private List<Happening> _filteredList;
        public List<Happening> FilteredList
        {
            get => _filteredList;
            set
            {
                _filteredList = value;
                OnPropertyChanged(() => FilteredList);
            }
        }

        private List<Happening> _filteredListWeek;
        public List<Happening> FilteredListWeek
        {
            get => _filteredListWeek;
            set
            {
                _filteredListWeek = value;
                OnPropertyChanged(() => FilteredListWeek);
            }
        }

        private List<HappeningsFilteredYearCategory> _datahappeningsByYear;
        public List<HappeningsFilteredYearCategory> DataHappeningsByYear
        {
            get => _datahappeningsByYear;
            set
            {
                _datahappeningsByYear = value;
                OnPropertyChanged(() => DataHappeningsByYear);
            }
        }

        private List<HappeningsFilteredWeekCategory> _datahappeningsByWeek;
        public List<HappeningsFilteredWeekCategory> DataHappeningsByWeek
        {
            get => _datahappeningsByWeek;
            set
            {
                _datahappeningsByWeek = value;
                OnPropertyChanged(() => DataHappeningsByWeek);
            }
        }

        private List<HappeningsFilteredWeekCategory> _datahappeningsByWeekFiltered;
        public List<HappeningsFilteredWeekCategory> DataHappeningsByWeekFiltered
        {
            get => _datahappeningsByWeekFiltered;
            set
            {
                _datahappeningsByWeekFiltered = value;
                OnPropertyChanged(() => DataHappeningsByWeekFiltered);
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

        private string _userName = MyGlobals.userLoggedIn;
        public string Username
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged(() => Username);
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

        private string _fullName;
        public string FullName
        {
            get => _fullName;
            set
            {
                _fullName = value;
                OnPropertyChanged(() => FullName);
            }
        }

        private string _workPlace;
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

        private string _saldo;
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

            //OMG filtering a list by creating other lists... somthing I think this SHOULD have been easier
            //I still struggle with this syntax - hoping future projects will be more streamlined
            var service = new DataService<Happening>(new TimeKeeprDbContextFactory());
            var UngroupedList = (List<Happening>)await service.GetAll();
            FilteredList = UngroupedList
                .Where(x => x.UserName.Contains(MyGlobals.userLoggedIn))
                .ToList();
            FilteredListWeek = FilteredList
                .Where(y => y.Category.Contains("WorkDay"))
                .ToList();

            DataHappeningsByYear = FilteredList
                .GroupBy(a => (a.Category, a.EventDate.Year))
                .Select(c => new HappeningsFilteredYearCategory
                {
                    Category = c.Key.Category,
                    Year = c.Key.Year,
                    TimeInHours = c.Sum(c => c.TimeInHours)
                }).ToList();

            DataHappeningsByWeek = FilteredList
                .GroupBy(a => (a.Category, a.EventDate.Year, a.WeekNr))
                .Select(c => new HappeningsFilteredWeekCategory
                {
                    Category = c.Key.Category,
                    Year = c.Key.Year,
                    WeekNr = c.Key.WeekNr,
                    TimeInHours = c.Sum(c => c.TimeInHours)
                }).ToList();

            DataHappeningsByWeekFiltered = FilteredListWeek
                .GroupBy(a => (a.Category, a.EventDate.Year, a.WeekNr))
                .Select(c => new HappeningsFilteredWeekCategory
                {
                    Category = c.Key.Category,
                    Year = c.Key.Year,
                    WeekNr = c.Key.WeekNr,
                    TimeInHours = c.Sum(c => c.TimeInHours),
                }).ToList();

            Saldo = (DataHappeningsByWeekFiltered.Sum(item => item.TimeInHours) - 
                (HoursPerWeek * DataHappeningsByWeekFiltered.Count)).ToString("F2") + " hours";
        }
    }
}