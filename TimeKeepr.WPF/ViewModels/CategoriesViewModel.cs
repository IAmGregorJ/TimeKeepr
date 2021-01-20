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

using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Windows.Input;
using TimeKeepr.Domain.Models;
using TimeKeepr.EntityFramework;
using TimeKeepr.EntityFramework.Services;
using TimeKeepr.WPF.Globals;
using TimeKeepr.WPF.Helper;
using TimeKeepr.WPF.Localizations;

namespace TimeKeepr.WPF.ViewModels
{
    public class CategoriesViewModel : BaseViewModel
    {
        #region catebories properties
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

        private bool _isActive = false;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                OnPropertyChanged(() => IsActive);
            }
        }
        #endregion
        #region helper properties
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
        #endregion

        ResourceManager rm = new ResourceManager(typeof(Resources));

        //constructor
        public CategoriesViewModel()
        {
            GetCategories();
        }

        public ICommand UpdateCommand => new BaseCommand(ClickUpdate);
        private async void ClickUpdate()
        {
            ButtonIsEnabled = "false";

            EventCategory categoryToUpdate = new EventCategory()
            {
                Id = SelectedCategory.Id,
                Category = SelectedCategory.Category,
                IsActive = SelectedCategory.IsActive
            };

            var service = new DataService<EventCategory>(new TimeKeeprDbContextFactory());
            EventCategory eventCategory = await service.Get(SelectedCategory.Id);
            if (eventCategory == null)
            {
                ShowMessageBox(rm.GetString("Category_notexist"));
            }
            else
            {
                eventCategory = await service.Update(SelectedCategory.Id, categoryToUpdate);
                ShowMessageBox(rm.GetString("Category_updated"));
            }

            GetCategories();
            ButtonIsEnabled = "true";
        }

        public ICommand AddCommand => new BaseCommand(ClickAdd);
        private async void ClickAdd()
        {
            ButtonIsEnabled = "false";

            Id = SelectedCategory.Id;
            Category = SelectedCategory.Category;
            IsActive = SelectedCategory.IsActive;

            //all of the following COULD be refactored and removed from the VM
            var serv = new DataService<User>(new TimeKeeprDbContextFactory());
            EventCategory cat = await serv.GetByCategoryName(Category);
            if (cat == null)
            {
                EventCategory eventCategory = new EventCategory()
                {
                    Category = _category,
                    IsActive = _isActive,
                    UserName = MyGlobals.userLoggedIn
                };
                var service = new DataService<EventCategory>(new TimeKeeprDbContextFactory());
                await service.Create(eventCategory);
                ShowMessageBox(rm.GetString("Category_added"));

                //GetCategories forces the list to update in the ListView - is there a better way to do this? Dunno. It works.
                GetCategories();
            }
            else
                ShowMessageBox(rm.GetString("Category_exists"));

            ButtonIsEnabled = "true";
        }

        public ICommand DeleteCommand => new BaseCommand(ClickDelete);

        private async void ClickDelete()
        {
            ButtonIsEnabled = "false";

            Id = SelectedCategory.Id;
            var service = new DataService<EventCategory>(new TimeKeeprDbContextFactory());
            await service.Delete(Id);
            ShowMessageBox(rm.GetString("Category_deleted"));

            GetCategories();
            ButtonIsEnabled = "true";
        }

        private async void GetCategories()
        {
            var service = new DataService<EventCategory>(new TimeKeeprDbContextFactory());
            var UnfilteredList = (List<EventCategory>)await service.GetAll();
            Categories = UnfilteredList
                .Where(x => !x.Category.Contains("WorkDay") && x.UserName == MyGlobals.userLoggedIn)
                .ToList();
            if (Categories.Count != 0)
                SelectedCategory = Categories.FirstOrDefault();
            else
            {
                SelectedCategory = new EventCategory()
                {
                    Id = 0,
                    Category = "",
                    IsActive = false,
                    UserName = MyGlobals.userLoggedIn
                };
            }
        }
    }
}