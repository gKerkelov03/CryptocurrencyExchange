using System.Windows;
using System.Windows.Input;
using Application.Abstractions;
using Presentation.Windows;

namespace Presentation.Commands;

public class LogoutCommand : ICommand
{
    private readonly IUserService _userService;
    private IBalanceService _balanceService;

    public LogoutCommand(IUserService userService, IBalanceService balanceService)
    {
        _userService = userService;
        _balanceService = balanceService;
    }

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
        var loginWindow = new LoginWindow(_userService, _balanceService);
        loginWindow.Show();
        System.Windows.Application.Current.MainWindow.Close();
        System.Windows.Application.Current.MainWindow = loginWindow;
    }
} 