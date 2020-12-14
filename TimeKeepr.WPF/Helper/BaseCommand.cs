using System;
using System.Windows.Input;

namespace TimeKeepr.WPF.Helper
{
    internal class BaseCommand : ICommand
    {
        private readonly Action command;
        private readonly Func<bool> canExecute;

        public BaseCommand(Action command, Func<bool> canExecute = null)
        {
            this.canExecute = canExecute;
            this.command = command ?? throw new ArgumentNullException("command");
        }

        public void Execute(object parameter)
        {
            command();
        }

        public bool CanExecute(object parameter)
        {
            if (canExecute == null)
                return true;
            return canExecute();
        }

        public event EventHandler CanExecuteChanged;
    }
}