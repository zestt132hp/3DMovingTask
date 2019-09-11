using System;
using System.Windows.Input;

namespace _3DMovingTask.Model
{
    class DelegateCommand:ICommand
    {
        private readonly Action _execute;

        private readonly Func<bool> _canExecute;

        public DelegateCommand(Action execute):this(execute, null) { }
        public DelegateCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }

            remove
            {
                if (_canExecute != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }
        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke() ?? true;
        }
        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
        public void Execute(object parameter)
        {
            _execute?.Invoke();
        }
        }
}
