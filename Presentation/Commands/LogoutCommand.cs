using System.Windows;
using System.Windows.Input;
using Application.Abstractions;
using Presentation.Windows;

namespace Presentation.Commands;

public class LogoutCommand : ICommand
{
    private readonly IUserService _userService;
    private readonly IBalanceService _balanceService;
    private readonly ICryptoPriceService cryptoPriceService;

    public LogoutCommand(IUserService userService, IBalanceService balanceService, ICryptoPriceService cryptoPriceService)
    {
        _userService = userService;
        _balanceService = balanceService;
        this.cryptoPriceService = cryptoPriceService;
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
        var loginWindow = new LoginWindow(_userService, _balanceService, cryptoPriceService);
        loginWindow.Show();
        System.Windows.Application.Current.MainWindow.Close();
        System.Windows.Application.Current.MainWindow = loginWindow;
    }
} 