using System.Windows;
using System.Windows.Input;

namespace Presentation.Commands;

public class ExitCommand : ICommand
{
    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter)
    {
        System.Windows.Application.Current.Shutdown();
    }
} 