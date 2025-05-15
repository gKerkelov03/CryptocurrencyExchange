using System.Windows;
using System.Windows.Controls;
using Application.Abstractions;
using Presentation.ViewModels;

namespace Presentation.Windows;

public partial class LoginWindow : Window, ITransientLifetime
{
    private readonly IUserService _userService;
    private readonly IBalanceService _balanceService;
    private readonly ICurrencyPriceService _currencyPriceService;

    public LoginWindow(IUserService userService, IBalanceService balanceService, ICurrencyPriceService currencyPriceService)
    {
        InitializeComponent();
        _userService = userService;
        _balanceService = balanceService;
        _currencyPriceService = currencyPriceService;
        DataContext = new LoginViewModel(userService, balanceService, currencyPriceService);
    }

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext is LoginViewModel viewModel)
        {
            viewModel.Password = PasswordBox.Password;
        }
    }
}
