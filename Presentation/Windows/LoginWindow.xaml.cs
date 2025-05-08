using System.Windows;
using System.Windows.Controls;
using Application.Abstractions;
using Presentation.ViewModels;

namespace Presentation.Windows;

public partial class LoginWindow : Window, ITransientLifetime
{
    private readonly IUserService _userService;

    public LoginWindow(IUserService userService)
    {
        InitializeComponent();
        _userService = userService;
        DataContext = new LoginViewModel(_userService);
    }

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext is LoginViewModel viewModel)
        {
            viewModel.Password = PasswordBox.Password;
        }
    }
}
