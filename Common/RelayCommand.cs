using System;
using System.Windows.Input;

namespace Common
{
    public class RelayCommand : ICommand
    {
        private Action<object> executeAction;
        private Predicate<object> canExecutePredicate;

        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            this.executeAction = execute;
            this.canExecutePredicate = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecutePredicate == null ? true : this.canExecutePredicate(parameter);
        }

        public void Execute(object parameter)
        {
            this.executeAction(parameter);
        }
    }
}
