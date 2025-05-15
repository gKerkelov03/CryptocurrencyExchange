using System.Windows;
using System.Windows.Input;
using Application.Abstractions;
using Presentation.Windows;

namespace Presentation.Commands;

public class LogoutCommand : ICommand
{
    private readonly IUserService _userService;
    private readonly IBalanceService _balanceService;
    private readonly ICurrencyPriceService currencyPriceService;

    public LogoutCommand(IUserService userService, IBalanceService balanceService, ICurrencyPriceService currencyPriceService)
    {
        _userService = userService;
        _balanceService = balanceService;
        this.currencyPriceService = currencyPriceService;
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
        var loginWindow = new LoginWindow(_userService, _balanceService, currencyPriceService);
        loginWindow.Show();
        System.Windows.Application.Current.MainWindow.Close();
        System.Windows.Application.Current.MainWindow = loginWindow;
    }
} 