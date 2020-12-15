﻿using TimeKeepr.WPF.State.Navigators;

namespace TimeKeepr.WPF.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public INavigator Navigator { get; set; } = new Navigator();
    }
}