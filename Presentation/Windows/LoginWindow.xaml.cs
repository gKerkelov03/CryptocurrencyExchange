using System.Windows;
using System.Windows.Controls;
using Application.Abstractions;
using Presentation.ViewModels;

namespace Presentation.Windows;

public partial class LoginWindow : Window, ITransientLifetime
{
    

    public LoginWindow(IUserService userService, IBalanceService balanceService)
    {
        InitializeComponent();
        DataContext = new LoginViewModel(userService, balanceService);
    }

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext is LoginViewModel viewModel)
        {
            viewModel.Password = PasswordBox.Password;
        }
    }
}
