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

using System.Reflection;
using System.Windows;

namespace TimeKeepr.WPF.Helper
{
    //NOT USED in the final solution - but I'm still playing around with it so not deleting quite yet
    public static class perWindowHelper
    {
        public static readonly DependencyProperty CloseWindowProperty = DependencyProperty.RegisterAttached(
            "CloseWindow",
            typeof(bool?),
            typeof(perWindowHelper),
            new PropertyMetadata(null, OnCloseWindowChanged));

        private static void OnCloseWindowChanged(DependencyObject target, DependencyPropertyChangedEventArgs args)
        {
            if (!(target is Window view))
            {
                return;
            }

            if (view.IsModal())
            {
                view.DialogResult = args.NewValue as bool?;
            }
            else
            {
                view.Close();
            }
        }

        public static void SetCloseWindow(Window target, bool? value)
        {
            target.SetValue(CloseWindowProperty, value);
        }

        public static bool IsModal(this Window window)
        {
            var fieldInfo = typeof(Window).GetField("_showingAsDialog", BindingFlags.Instance | BindingFlags.NonPublic);
            return fieldInfo != null && (bool)fieldInfo.GetValue(window);
        }
    }
}