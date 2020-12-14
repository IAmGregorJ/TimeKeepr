using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using TimeKeepr.Domain.Models;
using TimeKeepr.EntityFramework;
using TimeKeepr.EntityFramework.Services;
using TimeKeepr.WPF.Helper;

namespace TimeKeepr.WPF.ViewModels
{
    /*Oh crap.
     * I forgot that it MIGHT be a good idea to have these categories be user-specific.
     * As of now, all categories are visible/usable for all users.
     * My bad. Right now I'm too far along in the process, and have too much left to do to go back
     * and make it all good. Sorry!*/

    public class CategoriesViewModel : BaseViewModel
    {
        #region catebories property
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
        //constructor
        public CategoriesViewModel()
        {
            GetCategories(); 
        }

        public ICommand UpdateCommand { get { return new BaseCommand(ClickUpdate); } }
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
                ShowMessageBox("There is no such category. Please add a new one or chose another");
            }
            else
            {
                eventCategory = await service.Update(SelectedCategory.Id, categoryToUpdate);
                ShowMessageBox("The Category is updated.");
            }

            GetCategories();
            ButtonIsEnabled = "true";
        }

        public ICommand AddCommand { get { return new BaseCommand(ClickAdd); } }
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
                    IsActive = _isActive
                };
                var service = new DataService<EventCategory>(new TimeKeeprDbContextFactory());
                await service.Create(eventCategory);
                ShowMessageBox("The category is added.");

                //GetCategories forces the list to update in the ListView - is there a better way to do this? Dunno. It works.
                GetCategories();
            }
            else
                ShowMessageBox("A category by that name already exists. Please try another.");

            ButtonIsEnabled = "true";
        }

        public ICommand DeleteCommand { get { return new BaseCommand(ClickDelete); } }

        private async void ClickDelete()
        {
            ButtonIsEnabled = "false";

            Id = SelectedCategory.Id;
            var service = new DataService<EventCategory>(new TimeKeeprDbContextFactory());
            await service.Delete(Id);
            ShowMessageBox("The category is deleted");

            GetCategories();
            ButtonIsEnabled = "true";
        }

        //method SHOULD be removed to some sort of methods class with similar methods from other VMs
        private async void GetCategories()
        {
            var service = new DataService<EventCategory>(new TimeKeeprDbContextFactory());
            var UnfilteredList = (List<EventCategory>)await service.GetAll();
            Categories = UnfilteredList
                .Where(x => !x.Category.Contains("WorkDay"))
                .ToList();

            SelectedCategory = Categories.First();
        }
    }
}