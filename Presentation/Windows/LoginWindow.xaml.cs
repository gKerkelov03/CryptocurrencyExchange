using System.Windows;
using System.Windows.Controls;
using Application.Abstractions;
using Presentation.ViewModels;

namespace Presentation.Windows;

public partial class LoginWindow : Window, ITransientLifetime
{
    private readonly IUserService _userService;
    private readonly IBalanceService _balanceService;
    private readonly ICryptoPriceService _cryptoPriceService;

    public LoginWindow(IUserService userService, IBalanceService balanceService, ICryptoPriceService cryptoPriceService)
    {
        InitializeComponent();
        _userService = userService;
        _balanceService = balanceService;
        _cryptoPriceService = cryptoPriceService;
        DataContext = new LoginViewModel(userService, balanceService, cryptoPriceService);
    }

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext is LoginViewModel viewModel)
        {
            viewModel.Password = PasswordBox.Password;
        }
    }
}
