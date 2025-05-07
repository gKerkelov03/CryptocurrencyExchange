using System.Windows;
using System.Windows.Controls;
using Application.Abstractions;
using Presentation.ViewModels;

namespace Presentation.Windows;

public partial class LoginWindow : Window, ITransientLifetime
{
    public LoginWindow()
    {
        InitializeComponent();
        //DataContext = new LoginViewModel(userService);
    }

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext is LoginViewModel viewModel)
        {
            viewModel.Password = PasswordBox.Password;
        }
    }
}
