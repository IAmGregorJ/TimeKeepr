using System.Windows.Input;
using TimeKeepr.WPF.ViewModels;

namespace TimeKeepr.WPF.State.Navigators
{
    public enum ViewType
    {
        Home,
        Categories,
        Statistics,
        Help
    }

    public interface INavigator
    {
        BaseViewModel CurrentViewModel { get; set; }
        ICommand UpdateCurrentViewModelCommand { get; }
    }
}