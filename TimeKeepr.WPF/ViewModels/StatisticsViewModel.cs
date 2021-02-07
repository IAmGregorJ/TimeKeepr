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
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Resources;
using System.Windows.Input;
using ClosedXML.Excel;
using GJDateTime;
using TimeKeepr.Domain.Models;
using TimeKeepr.EntityFramework;
using TimeKeepr.EntityFramework.Services;
using TimeKeepr.WPF.Globals;
using TimeKeepr.WPF.Helper;
using TimeKeepr.WPF.Localizations;

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
        private int _userId;
        public int UserId
        {
            get => _userId;
            set
            {
                _userId = value;
                OnPropertyChanged(() => UserId);
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

        #region FlexTime properties

        #endregion
        ResourceManager rm = new ResourceManager(typeof(Resources));

        //constructor
        public StatisticsViewModel()
        {
            GetCategories();
        }

        public ICommand ClickRefresh => new BaseCommand(GetCategories);
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
            UserId = user.Id;

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
                })
                .OrderByDescending(a => (a.Year))
                .ThenByDescending(a => (a.Category))
                .ThenByDescending(a => (a.WeekNr)).ToList();

            WorkHoursWeek = UngroupedList
                .Where(x => x.UserName.Contains(MyGlobals.userLoggedIn) && x.Category.Contains("WorkDay") && x.EventDate <= DateTime.Now)
                .GroupBy(a => (a.Category, a.Year, a.WeekNr))
                .Select(c => new Happening
                {
                    Category = c.Key.Category,
                    Year = c.Key.Year,
                    WeekNr = c.Key.WeekNr,
                    TimeInHours = c.Sum(c => c.TimeInHours)
                })
                .OrderByDescending(a => (a.Year))
                .ThenByDescending(a => (a.WeekNr)).ToList();


            HoursInMeeting = UngroupedList
                .Where(x => x.UserName.Contains(MyGlobals.userLoggedIn) && !x.Category.Contains("WorkDay"))
                .GroupBy(a => a.Category)
                .Select(c => new Happening
                {
                    Category = c.Key,
                    TimeInHours = c.Sum(c => c.TimeInHours),
                    IsMeetingHours = c.Sum(c => c.IsMeetingHours)
                })
                .OrderByDescending(a => (a.Category)).ToList();

            //If I ever need to filter something like Saldo a bit more... percentage spent in meetings
            //##############################################################
            //var TimeSpentInMeetingsThisMonth = UngroupedList
            //    .Where(x => x.UserName.Contains(MyGlobals.userLoggedIn) && !x.Category.Contains("WorkDay"))
            //    .Where(a => a.IsMeeting)
            //    .Where(a => a.EventDate.Month == DateTime.Now.Month)
            //    .Sum(a => a.TimeInHours);

            //var TimeSpentOnProjectsThisMonth = UngroupedList
            //    .Where(x => x.UserName.Contains(MyGlobals.userLoggedIn) && !x.Category.Contains("WorkDay"))
            //    .Where(a => a.EventDate.Month == DateTime.Now.Month)
            //    .Sum(a => a.TimeInHours);

            //var PercentSpentInMeetingsThisMonth = ((TimeSpentInMeetingsThisMonth / TimeSpentOnProjectsThisMonth) * 100).ToString("P");
            //##############################################################

            var serviceFlex = new DataService<FlexTime>(new TimeKeeprDbContextFactory());

            var FlexWeek = WorkHoursWeek
                .Where(x => !(x.Year == DateTime.Now.Year && x.WeekNr == WeekNumber.GetIso8601WeekOfYear(DateTime.Now))).ToList();
            foreach (Happening happening in FlexWeek)
            {
                UserName = _userName;
                FlexTime flexTime = new FlexTime()
                {
                    Year = happening.Year,
                    WeekNr = happening.WeekNr,
                    TotalHoursWeek = happening.TimeInHours,
                    HoursPerWeek = MyGlobals.usersHours,
                    FlexHours = happening.TimeInHours - HoursPerWeek,
                    UserName = MyGlobals.userLoggedIn
                };
                if (await serviceFlex.GetFlexByYearWeekNr(flexTime.Year, flexTime.WeekNr) == null)
                    await serviceFlex.Create(flexTime);
            }

            var flexList = (List<FlexTime>)await serviceFlex.GetAll();
            var flextime = flexList
                .Where(u => u.UserName == MyGlobals.userLoggedIn)
                .Sum(c => c.FlexHours);

            var flexTimeT = flextime + PreviousSaldo;
            var flexTimeTotal = Math.Round(Convert.ToDecimal(flexTimeT), 2);

            Saldo = Convert.ToString(flexTimeTotal);
        }

        public ICommand UpdateUser => new BaseCommand(UpdateHoursPerWeek);
        private async void UpdateHoursPerWeek()
        {

            var serviceUpdate = new DataService<User>(new TimeKeeprDbContextFactory());
            User user = await serviceUpdate.Get(UserId);
            User userToUpdate = new User()
            {
                Id = user.Id,
                UserName = user.UserName,
                EMail = user.EMail,
                FirstName = user.FirstName,
                LastName = user.LastName,
                WorkPlace = user.WorkPlace,
                HoursPerWeek = HoursPerWeek,
                PreviousSaldo = user.PreviousSaldo,
                Salt = user.Salt,
                PasswordHash = user.PasswordHash
            };

            user = await serviceUpdate.Update(UserId, userToUpdate);
        }

        #region Excel
        public ICommand CreateXL => new BaseCommand(ClickToXL);

        //Export data to Excel file
        private async void ClickToXL()
        {
            //string path = "TimeKeepr.xlsx";
            string path = "TimeKeepr - " +
                DateTime.Now.Year +
                DateTime.Now.Month.ToString("d2") +
                DateTime.Now.Day.ToString("d2") +
                DateTime.Now.Hour.ToString("d2") +
                DateTime.Now.Minute.ToString("d2") +
                ".xlsx";
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Summary");
            var ws2 = wb.Worksheets.Add("All Data");
            var service = new DataService<Happening>(new TimeKeeprDbContextFactory());
            var UngroupedList = (List<Happening>)await service.GetAll();
            var FullList = UngroupedList
                .Where(x => x.UserName.Contains(MyGlobals.userLoggedIn));
            var WorkHoursList = UngroupedList
                .Where(x => x.UserName.Contains(MyGlobals.userLoggedIn) && x.Category.Contains("WorkDay"))
                .GroupBy(a => (a.Year, a.WeekNr))
                .Select(c => new {
                    Year = c.Key.Year,
                    WeekNr = c.Key.WeekNr,
                    TimeInHours = c.Sum(c => c.TimeInHours)
                })
                .OrderByDescending(a => (a.Year))
                .ThenByDescending(a => (a.WeekNr));

            var CategoryList = UngroupedList
                .Where(x => x.UserName.Contains(MyGlobals.userLoggedIn) && !x.Category.Contains("WorkDay"))
                .GroupBy(a => (a.Category))
                .Select(c => new {
                    Category = c.Key,
                    IsMeetingHours = c.Sum(c => c.IsMeetingHours),
                    TimeInHours = c.Sum(c => c.TimeInHours)
                })
                .OrderBy(a => a.Category);

            ws.Cell(3, 1).Value = rm.GetString("FullName_txt");
            ws.Cell(3, 2).Value = FullName;
            ws.Cell(4, 1).Value = rm.GetString("Workplace_txt");
            ws.Cell(4, 2).Value = WorkPlace;
            ws.Cell(5, 1).Value = rm.GetString("HoursPerWeek_txt");
            ws.Cell(5, 2).Value = HoursPerWeek;
            ws.Cell(6, 1).Value = rm.GetString("Balance_txt");
            ws.Cell(6, 2).Value = Convert.ToDouble(Saldo);

            var range1 = ws.Range("A3:B6").AddToNamed("StamData");
            range1.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
            range1.Style.Border.OutsideBorderColor = XLColor.Orange;
            range1.Style.Fill.BackgroundColor = XLColor.LightGreen;

            DataTable hw = ConvertToDataTable.ToDataTable(WorkHoursList);
            DataTable hc = ConvertToDataTable.ToDataTable(CategoryList);
            DataTable ht = ConvertToDataTable.ToDataTable(FullList);
            ws.Cell(1, 1).Value = DateTime.Now;
            ws2.Cell(1, 1).Value = DateTime.Now;
            ws.Cell(8, 1).Value = rm.GetString("Work_hours");
            ws.Cell(8, 1).Style.Fill.BackgroundColor = XLColor.LightGreen;
            ws.Cell(8, 5).Value = rm.GetString("Category_hours");
            ws.Cell(8, 5).Style.Fill.BackgroundColor = XLColor.LightGreen;

            var tableWithData = ws.Cell(9, 1).InsertTable(hw.AsEnumerable());
            tableWithData.ShowTotalsRow = true;
            tableWithData.Field("TimeInHours").TotalsRowFunction = XLTotalsRowFunction.Sum;
            tableWithData.Field(0).TotalsRowLabel = "Total Hours";
            var tableWithDataCategories = ws.Cell(9, 5).InsertTable(hc.AsEnumerable());
            tableWithDataCategories.ShowTotalsRow = true;
            tableWithDataCategories.Field("TimeInHours").TotalsRowFunction = XLTotalsRowFunction.Sum;
            tableWithDataCategories.Field("IsMeetingHours").TotalsRowFunction = XLTotalsRowFunction.Sum;
            tableWithDataCategories.Field(0).TotalsRowLabel = "Total Hours";
            var tableWithAllData = ws2.Cell(3, 1).InsertTable(ht.AsEnumerable());
            tableWithAllData.ShowTotalsRow = true;
            tableWithAllData.Field("TimeInHours").TotalsRowFunction = XLTotalsRowFunction.Sum;
            tableWithAllData.Field("IsMeetingHours").TotalsRowFunction = XLTotalsRowFunction.Sum;
            tableWithAllData.Field(0).TotalsRowLabel = "Total Hours";

            //TODO Flex week by week table to xlsx

            ws.Columns().AdjustToContents();
            ws2.Columns().AdjustToContents();

            using (var stream = File.OpenWrite(path))
            {
                wb.SaveAs(stream);
            }
            Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
            wb.Dispose();
        }
        #endregion
    }
}