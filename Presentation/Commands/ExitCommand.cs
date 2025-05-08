using System.Windows;
using System.Windows.Input;

namespace Presentation.Commands;

public class ExitCommand : ICommand
{
    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Execute(object parameter)
    {
        System.Windows.Application.Current.Shutdown();
    }
} 