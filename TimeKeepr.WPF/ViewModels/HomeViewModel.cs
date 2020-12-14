﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using TimeKeepr.Domain.Models;
using TimeKeepr.EntityFramework;
using TimeKeepr.EntityFramework.Services;
using TimeKeepr.WPF.Globals;
using TimeKeepr.WPF.Helper;

namespace TimeKeepr.WPF.ViewModels
{
    //This really shouldn't be named HomeViewModel - but it was a lot the last moment that I renamed the section to "Logging"
    public class HomeViewModel : BaseViewModel
    {
        #region Happenings properties
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

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                OnPropertyChanged(() => IsActive);
            }
        }

        private string _userName = MyGlobals.userLoggedIn; 
        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged(() => Id);
            }
        }

        private bool _isMeeting = false; //default is needed in order to avoid null values - null values for me are just messy
        public bool IsMeeting
        {
            get => _isMeeting;
            set
            {
                _isMeeting = value;
                OnPropertyChanged(() => IsMeeting);
            }
        }

        //Time in hours uses TimeHeelper assembly:
        //public static double NumberOfHoursElapsed(DateTime start, DateTime stop)
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

        //today's date - maybe good enough just to put DateTime.Now in there??
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

        //Iso9601 format - from TImeHelper:
        //public static int GetIso8601WeekOfYear(DateTime time)
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
        #endregion Happenings properties
        #region ButtonIsEnabled
        //different property for each button - is there another way?
        private string _stbuttonIsEnabled = "true";
        public string StButtonIsEnabled
        {
            get => _stbuttonIsEnabled;
            set
            {
                _stbuttonIsEnabled = value;
                OnPropertyChanged(() => StButtonIsEnabled);
            }
        }

        private string _spbuttonIsEnabled = "true";
        public string SpButtonIsEnabled
        {
            get => _spbuttonIsEnabled;
            set
            {
                _spbuttonIsEnabled = value;
                OnPropertyChanged(() => SpButtonIsEnabled);
            }
        }

        private string _stwbuttonIsEnabled = "true";
        public string StwButtonIsEnabled
        {
            get => _stwbuttonIsEnabled;
            set
            {
                _stwbuttonIsEnabled = value;
                OnPropertyChanged(() => StwButtonIsEnabled);
            }
        }

        private string _spwbuttonIsEnabled = "true";
        public string SpwButtonIsEnabled
        {
            get => _spwbuttonIsEnabled;
            set
            {
                _spwbuttonIsEnabled = value;
                OnPropertyChanged(() => SpwButtonIsEnabled);
            }
        }
        #endregion ButtonIsEnabled
        #region Start/Stop time

        private DateTime _startTime = new DateTime(2000, 01, 01);
        public DateTime StartTime
        {
            get => _startTime;
            set
            {
                _startTime = value;
                OnPropertyChanged(() => StartTime);
            }
        }

        private DateTime _startTimeWork = new DateTime(2000, 01, 01);
        public DateTime StartTimeWork
        {
            get => _startTimeWork;
            set
            {
                _startTimeWork = value;
                OnPropertyChanged(() => StartTimeWork);
            }
        }

        private DateTime _stopTime = new DateTime(2222, 02, 02);
        public DateTime StopTime
        {
            get => _stopTime;
            set
            {
                _stopTime = value;
                OnPropertyChanged(() => StopTime);
            }
        }

        private DateTime _stopTimeWork = new DateTime(2222, 02, 02);
        public DateTime StopTimeWork
        {
            get => _stopTimeWork;
            set
            {
                _stopTimeWork = value;
                OnPropertyChanged(() => StopTimeWork);
            }
        }
        #endregion Start/Stop time
        #region Combobox
        //List<T> to populate the ComboBox
        private List<EventCategory> _categories;
        public List<EventCategory> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged(() => Categories);
            }
        }

        //elements of the ComboBox
        private EventCategory _selectedCategory;
        public EventCategory SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged(() => SelectedCategory);
            }
        }
        #endregion Combobox

        //constructor
        public HomeViewModel()
        {
            GetCategories();
        }

        public ICommand StartCommandWork { get { return new BaseCommand(ClickStartWork); } }
        private void ClickStartWork()
        {
            StwButtonIsEnabled = "false";
            StartTimeWork = DateTime.Now;
        }

        public ICommand StopCommandWork { get { return new BaseCommand(ClickStopWork); } }
        private async void ClickStopWork()
        {
            SpwButtonIsEnabled = "false";
            StopTimeWork = DateTime.Now;

            Happening happening = new Happening()
            {
                Id = _id,
                Category = "WorkDay",
                UserName = MyGlobals.userLoggedIn,
                IsMeeting = _isMeeting,
                TimeInHours = TimeHelper.TimeHelper.NumberOfHoursElapsed(StartTimeWork, StopTimeWork),
                EventDate = DateTime.Now,
                WeekNr = TimeHelper.TimeHelper.GetIso8601WeekOfYear(DateTime.Now)
            };

            var service = new DataService<Happening>(new TimeKeeprDbContextFactory());
            await service.Create(happening);

            //terminate any running projects
            ClickStop();

            StwButtonIsEnabled = "true";
            SpwButtonIsEnabled = "true";
            StartTimeWork = new DateTime(2000, 01, 01);
            StopTimeWork = new DateTime(2000, 01, 01);
        }

        public ICommand StartCommand { get { return new BaseCommand(ClickStart); } }
        private void ClickStart()
        {
            if (StwButtonIsEnabled == "true")
                ShowMessageBox("You have to begin your Work Day before you can start a project");
            else
            {
                if (SelectedCategory == null)
                {
                    ShowMessageBox("You must choose a category before beginning");
                }
                else
                {
                    StButtonIsEnabled = "false";
                    StartTime = DateTime.Now;
                }
            }
        }

        public ICommand StopCommand { get { return new BaseCommand(ClickStop); } }
        private async void ClickStop()
        {
            SpButtonIsEnabled = "false";
            StopTime = DateTime.Now;
            Category = SelectedCategory.Category;

            Happening happening = new Happening()
            {
                Id = _id,
                Category = _category,
                UserName = MyGlobals.userLoggedIn,
                IsMeeting = _isMeeting,
                TimeInHours = TimeHelper.TimeHelper.NumberOfHoursElapsed(StartTime, StopTime),
                EventDate = DateTime.Now,
                WeekNr = TimeHelper.TimeHelper.GetIso8601WeekOfYear(DateTime.Now)
            };

            var service = new DataService<Happening>(new TimeKeeprDbContextFactory());
            await service.Create(happening);
            StButtonIsEnabled = "true";
            SpButtonIsEnabled = "true";
            StartTime = new DateTime(2000, 01, 01);
            StopTime = new DateTime(2000, 01, 01);
        }

        private async void GetCategories()
        {
            var service = new DataService<EventCategory>(new TimeKeeprDbContextFactory());
            var UnfilteredList = (List<EventCategory>)await service.GetAll();
            Categories = UnfilteredList
                .Where(x => x.IsActive)
                .Where(x => !x.Category.Contains("WorkDay"))
                .ToList();
        }
    }
}