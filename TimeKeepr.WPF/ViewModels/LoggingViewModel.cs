﻿// This file is part of TimeKeepr.
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
using System.IO;
using System.Linq;
using System.Resources;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Input;
using GJDateTime;
using TimeKeepr.Domain.Models;
using TimeKeepr.EntityFramework;
using TimeKeepr.EntityFramework.Services;
using TimeKeepr.WPF.Globals;
using TimeKeepr.WPF.Helper;
using TimeKeepr.WPF.Localizations;

namespace TimeKeepr.WPF.ViewModels
{
    //This really shouldn't be named LoggingViewModel - but it was a lot the last moment that I renamed the section to "Logging"
    public class LoggingViewModel : BaseViewModel
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

        private double _isMeetingHours = 0; //default is needed in order to avoid null values - null values for me are just messy
        public double IsMeetingHours
        {
            get => _isMeetingHours;
            set
            {
                _isMeetingHours = value;
                OnPropertyChanged(() => IsMeetingHours);
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

        //Iso9601 format - from WeekNumber assembly:
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

        private string _regwbuttonIsEnabled = "true";
        public string RegwButtonIsEnabled
        {
            get => _regwbuttonIsEnabled;
            set
            {
                _regwbuttonIsEnabled = value;
                OnPropertyChanged(() => RegwButtonIsEnabled);
            }
        }
        private string _regbuttonIsEnabled = "true";
        public string RegButtonIsEnabled
        {
            get => _regbuttonIsEnabled;
            set
            {
                _regbuttonIsEnabled = value;
                OnPropertyChanged(() => RegButtonIsEnabled);
            }
        }
        #endregion ButtonIsEnabled
        #region Start/Stop time
        private DateTime _startTime = DateTime.MinValue;
        public DateTime StartTime
        {
            get => _startTime;
            set
            {
                _startTime = value;
                OnPropertyChanged(() => StartTime);
            }
        }

        private DateTime _startTimeWork = DateTime.MinValue;
        public DateTime StartTimeWork
        {
            get => _startTimeWork;
            set
            {
                _startTimeWork = value;
                OnPropertyChanged(() => StartTimeWork);
            }
        }

        private DateTime _stopTime = DateTime.MaxValue;
        public DateTime StopTime
        {
            get => _stopTime;
            set
            {
                _stopTime = value;
                OnPropertyChanged(() => StopTime);
            }
        }

        private DateTime _stopTimeWork = DateTime.MaxValue;
        public DateTime StopTimeWork
        {
            get => _stopTimeWork;
            set
            {
                _stopTimeWork = value;
                OnPropertyChanged(() => StopTimeWork);
            }
        }

        private DateTime _dateWork = DateTime.Now;
        public DateTime DateWork
        {
            get => _dateWork;
            set
            {
                _dateWork = value;
                OnPropertyChanged(() => DateWork);
            }
        }

        private DateTime _dateTask = DateTime.Now;
        public DateTime DateTask
        {
            get => _dateTask;
            set
            {
                _dateTask = value;
                OnPropertyChanged(() => DateTask);
            }
        }
        #endregion Start/Stop time
        #region Combobox properties
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
        #region Hours/Minutes
        public IReadOnlyList<int> Hours { get; } = Enumerable.Range(0, 24).ToList();

        public IReadOnlyList<int> Minutes { get; } = Enumerable.Range(0, 4).Select(x => x * 15).ToList();

        private int _selectedHourStW;
        public int SelectedHourStW
        {
            get => _selectedHourStW;
            set
            {
                _selectedHourStW = value;
                OnPropertyChanged(() => SelectedHourStW);
            }
        }

        private int _selectedMinuteStW;
        public int SelectedMinuteStW
        {
            get => _selectedMinuteStW;
            set
            {
                _selectedMinuteStW = value;
                OnPropertyChanged(() => SelectedMinuteStW);
            }
        }
        private int _selectedHourSpW;
        public int SelectedHourSpW
        {
            get => _selectedHourSpW;
            set
            {
                _selectedHourSpW = value;
                OnPropertyChanged(() => SelectedHourSpW);
            }
        }

        private int _selectedMinuteSpW;
        public int SelectedMinuteSpW
        {
            get => _selectedMinuteSpW;
            set
            {
                _selectedMinuteSpW = value;
                OnPropertyChanged(() => SelectedMinuteSpW);
            }
        }

        private int _selectedHourStT;
        public int SelectedHourStT
        {
            get => _selectedHourStT;
            set
            {
                _selectedHourStT = value;
                OnPropertyChanged(() => SelectedHourStT);
            }
        }

        private int _selectedMinuteStT;
        public int SelectedMinuteStT
        {
            get => _selectedMinuteStT;
            set
            {
                _selectedMinuteStT = value;
                OnPropertyChanged(() => SelectedMinuteStT);
            }
        }
        private int _selectedHourSpT;
        public int SelectedHourSpT
        {
            get => _selectedHourSpT;
            set
            {
                _selectedHourSpT = value;
                OnPropertyChanged(() => SelectedHourSpT);
            }
        }

        private int _selectedMinuteSpT;
        public int SelectedMinuteSpT
        {
            get => _selectedMinuteSpT;
            set
            {
                _selectedMinuteSpT = value;
                OnPropertyChanged(() => SelectedMinuteSpT);
            }
        }
        #endregion

        ResourceManager rm = new ResourceManager(typeof(Resources));
        string fileName = "SaveData.dat";
        //constructor
        public LoggingViewModel()
        {
            GetCategories();

            if (File.Exists(fileName))
            {
                PDeserialize();
                ShowMessageBox("You exited the program without saving your data - it has been restored as it was.");
            }

            else
            {
                SpButtonIsEnabled = "false";
                SpwButtonIsEnabled = "false";
                StButtonIsEnabled = "false";
                RegwButtonIsEnabled = "false";
                RegButtonIsEnabled = "false";
                SelectedHourStW = Hours.FirstOrDefault();
                SelectedMinuteStW = Minutes.FirstOrDefault();
                SelectedHourSpW = Hours.FirstOrDefault();
                SelectedMinuteSpW = Minutes.FirstOrDefault();
                SelectedHourStT = Hours.FirstOrDefault();
                SelectedMinuteStT = Minutes.FirstOrDefault();
                SelectedHourSpT = Hours.FirstOrDefault();
                SelectedMinuteSpT = Minutes.FirstOrDefault();
            }
        }

        public ICommand StartCommandWork => new BaseCommand(ClickStartWork);
        private void ClickStartWork()
        {
            SelectedMinuteStW = (int)(Math.Round(DateTime.Now.Minute / 15.0) * 15 % 60);
            if (DateTime.Now.Minute > 45 && SelectedMinuteStW == 0)
                SelectedHourStW = DateTime.Now.Hour + 1;
            else
                SelectedHourStW = DateTime.Now.Hour;
            StwButtonIsEnabled = "false";
            SpwButtonIsEnabled = "true";
            StButtonIsEnabled = "true";

            PSerialize();
        }

        public ICommand StopCommandWork => new BaseCommand(ClickStopWork);
        private void ClickStopWork()
        {
            SelectedMinuteSpW = (int)(Math.Round(DateTime.Now.Minute / 15.0) * 15 % 60);
            if (DateTime.Now.Minute > 45 && SelectedMinuteSpW == 0)
                SelectedHourSpW = DateTime.Now.Hour + 1;
            else
                SelectedHourSpW = DateTime.Now.Hour;
            SpwButtonIsEnabled = "false";
            StButtonIsEnabled = "false";
            RegwButtonIsEnabled = "true";
            RegButtonIsEnabled = "false";
            PDelete();
            PSerialize();
        }

        public ICommand RegisterCommandWork => new BaseCommand(ClickRegisterWork);
        private async void ClickRegisterWork()
        {
            StartTimeWork = DateWork + new TimeSpan(SelectedHourStW, SelectedMinuteStW, 0);
            StopTimeWork = DateWork + new TimeSpan(SelectedHourSpW, SelectedMinuteSpW, 0);

            if (StartTimeWork == DateTime.MinValue ||
                    StopTimeWork == DateTime.MaxValue ||
                    StartTimeWork.TimeOfDay > StopTimeWork.TimeOfDay)
                ShowMessageBox(rm.GetString("Time_error"));
            else
            {
                RegwButtonIsEnabled = "false";
                Happening happening = new Happening()
                {
                    Id = _id,
                    Category = "WorkDay",
                    UserName = MyGlobals.userLoggedIn,
                    TimeInHours = (StopTimeWork - StartTimeWork).TotalHours,
                    EventDate = DateWork,
                    Year = DateWork.Year,
                    WeekNr = WeekNumber.GetIso8601WeekOfYear(DateWork)
                };
                var service = new DataService<Happening>(new TimeKeeprDbContextFactory());
                await service.Create(happening);

                StwButtonIsEnabled = "true";
                SpwButtonIsEnabled = "false";
                RegwButtonIsEnabled = "false";
                RegButtonIsEnabled = "false";
                StButtonIsEnabled = "false";
                SelectedHourStW = Hours.FirstOrDefault();
                SelectedMinuteStW = Minutes.FirstOrDefault();
                SelectedHourSpW = Hours.FirstOrDefault();
                SelectedMinuteStW = Minutes.FirstOrDefault();
                StartTimeWork = DateTime.MinValue;
                StopTimeWork = DateTime.MaxValue;

                PDelete();
            }
        }

        public ICommand StartCommand => new BaseCommand(ClickStart);
        private void ClickStart()
        {
            if (StwButtonIsEnabled == "true")
                ShowMessageBox(rm.GetString("WorkBegin_error"));
            else
            {
                if (SelectedCategory == null)
                {
                    ShowMessageBox(rm.GetString("CategoryChoose_error"));
                }
                else
                {
                    SelectedMinuteStT = (int)(Math.Round(DateTime.Now.Minute / 15.0) * 15 % 60);
                    if (DateTime.Now.Minute > 45 && SelectedMinuteStT == 0)
                        SelectedHourStT = DateTime.Now.Hour + 1;
                    else
                        SelectedHourStT = DateTime.Now.Hour;

                    StButtonIsEnabled = "false";
                    SpButtonIsEnabled = "true";
                    SpwButtonIsEnabled = "false";

                    PDelete();
                    PSerialize();
                }
            }
        }

        public ICommand StopCommand => new BaseCommand(ClickStop);
        private void ClickStop()
        {
            SelectedMinuteSpT = (int)(Math.Round(DateTime.Now.Minute / 15.0) * 15 % 60);
            if (DateTime.Now.Minute > 45 && SelectedMinuteSpT == 0)
                SelectedHourSpT = DateTime.Now.Hour + 1;
            else
                SelectedHourSpT = DateTime.Now.Hour;

            SpButtonIsEnabled = "false";
            RegButtonIsEnabled = "true";

            PDelete();
            PSerialize();
        }

        public ICommand RegisterCommandTask => new BaseCommand(ClickRegisterTask);
        private async void ClickRegisterTask()
        {
            StartTime = DateTask + new TimeSpan(SelectedHourStT, SelectedMinuteStT, 0);
            StopTime = DateTask + new TimeSpan(SelectedHourSpT, SelectedMinuteSpT, 0);

            if (StartTime == DateTime.MinValue || StopTime == DateTime.MaxValue || StartTime.TimeOfDay > StopTime.TimeOfDay)
                ShowMessageBox(rm.GetString("Time_error"));
            else
            {
                RegButtonIsEnabled = "false";

                Category = SelectedCategory.Category;
                if (IsMeeting)
                    IsMeetingHours = (StopTime - StartTime).TotalHours;
                Happening happening = new Happening()
                {
                    Id = _id,
                    Category = _category,
                    UserName = MyGlobals.userLoggedIn,
                    IsMeeting = _isMeeting,
                    IsMeetingHours = _isMeetingHours,
                    TimeInHours = (StopTime - StartTime).TotalHours,
                    EventDate = DateTask,
                    Year = DateTask.Year,
                    WeekNr = WeekNumber.GetIso8601WeekOfYear(DateTask)
                };

                var service = new DataService<Happening>(new TimeKeeprDbContextFactory());
                await service.Create(happening);
                SpwButtonIsEnabled = "true";
                if (StwButtonIsEnabled == "false" && SpwButtonIsEnabled == "true")
                    StButtonIsEnabled = "true";
                SelectedHourStT = Hours.FirstOrDefault();
                SelectedMinuteStT = Minutes.FirstOrDefault();
                SelectedHourSpT = Hours.FirstOrDefault();
                SelectedMinuteSpT = Minutes.FirstOrDefault();
                StartTime = DateTime.MinValue;
                StopTime = DateTime.MaxValue;

                PDelete();
                GetCategories();
                PSerialize();
            }
        }

        public ICommand RefreshCategories => new BaseCommand(GetCategories);
        private async void GetCategories()
        {
            var service = new DataService<EventCategory>(new TimeKeeprDbContextFactory());
            var UnfilteredList = (List<EventCategory>)await service.GetAll();
            Categories = UnfilteredList
                .Where(x => x.IsActive)
                .Where(x => !x.Category.Contains("WorkDay") && x.UserName == MyGlobals.userLoggedIn)
                .ToList();
        }

        public void PSerialize()
        {
            PersistentData pData = new PersistentData()
            {
                StButtonIsEnabledP = StButtonIsEnabled,
                StwButtonIsEnabledP = StwButtonIsEnabled,
                SpButtonIsEnabledP = SpButtonIsEnabled,
                SpwButtonIsEnabledP = SpwButtonIsEnabled,
                RegButtonIsEnabledP = RegButtonIsEnabled,
                RegwButtonIsEnabledP = RegwButtonIsEnabled,
                SelectedHourStWP = SelectedHourStW,
                SelectedMinuteStWP = SelectedMinuteSpW,
                SelectedHourSpWP = SelectedHourSpW,
                SelectedMinuteSpWP = SelectedMinuteSpW,
                SelectedHourStTP = SelectedHourStT,
                SelectedMinuteStTP = SelectedMinuteStT,
                SelectedHourSpTP = SelectedHourSpT,
                SelectedMinuteSpTP = SelectedMinuteSpT,
                DateWorkP = DateWork,
                DateTaskP = DateTask,
                CategoryP = Category,
                IsMeetingP = IsMeeting
            };
            BinaryFormatter binFormat = new BinaryFormatter();
            using (Stream fStream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                binFormat.Serialize(fStream, pData);
            }
        }

        public void PDeserialize()
        {
            BinaryFormatter binFormat = new BinaryFormatter();
            PersistentData pData = default;
            using (Stream fStream = File.OpenRead(fileName))
            {
                pData = (PersistentData)binFormat.Deserialize(fStream);
            }
            StButtonIsEnabled = pData.StButtonIsEnabledP;
            StwButtonIsEnabled = pData.StwButtonIsEnabledP;
            SpButtonIsEnabled = pData.SpButtonIsEnabledP;
            SpwButtonIsEnabled = pData.SpwButtonIsEnabledP;
            RegButtonIsEnabled = pData.RegButtonIsEnabledP;
            RegwButtonIsEnabled = pData.RegwButtonIsEnabledP;
            SelectedHourStW = pData.SelectedHourStWP;
            SelectedMinuteStW = pData.SelectedMinuteSpWP;
            SelectedHourSpW = pData.SelectedHourSpWP;
            SelectedMinuteSpW = pData.SelectedMinuteSpWP;
            SelectedHourStT = pData.SelectedHourStTP;
            SelectedMinuteStT = pData.SelectedMinuteStTP;
            SelectedHourSpT = pData.SelectedHourSpTP;
            SelectedMinuteSpT = pData.SelectedMinuteSpTP;
            DateWork = pData.DateWorkP;
            DateTask = pData.DateTaskP;
            Category = pData.CategoryP;
            IsMeeting = pData.IsMeetingP;
            SelectedCategory = Categories.Find(x => x.Category == pData.CategoryP);
        }

        public void PDelete()
        {
            File.Delete(fileName);
        }
    }
}