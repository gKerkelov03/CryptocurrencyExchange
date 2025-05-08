using System.Windows;
using System.Windows.Input;

namespace Presentation.Commands;

public class TradeCommand : ICommand
{
    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter)
    {
        MessageBox.Show("Trading functionality will be implemented soon!", "Coming Soon", MessageBoxButton.OK, MessageBoxImage.Information);
    }
} 