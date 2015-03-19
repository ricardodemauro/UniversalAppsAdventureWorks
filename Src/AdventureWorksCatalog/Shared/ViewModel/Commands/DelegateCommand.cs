using System;
using System.Windows.Input;

namespace AdventureWorksCatalog.ViewModel.Commands
{
    public class DelegateCommand : ICommand
    {
        public Action<object> ExecuteAction { get; set; }

        public Func<object, bool> CanExecuteFunction { get; set; }

        public DelegateCommand(Action<object> executeAction, Func<object, bool> canExecuteFunction = null)
        {
            if (canExecuteFunction == null)
                canExecuteFunction = (o) => true;
            if (executeAction == null)
                throw new ArgumentNullException("executeAction");

            ExecuteAction = executeAction;
            CanExecuteFunction = canExecuteFunction;
        }

        #region ICommand implementation
        public bool CanExecute(object parameter)
        {
            return CanExecuteFunction(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            ExecuteAction(parameter);
        }
        #endregion ICommand implementation
    }
}
