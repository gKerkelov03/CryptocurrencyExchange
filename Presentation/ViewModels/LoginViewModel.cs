using System.Windows;
using System.Windows.Input;
using Application.Abstractions;
using Application.Domain;
using Presentation.Commands;
using Presentation.Windows;
using SmartSalon.Application.ResultObject;

namespace Presentation.ViewModels;

public class LoginViewModel : ViewModelBase, ITransientLifetime
{
    private readonly IUserService _userService;
    private string _username = string.Empty;
    private string _password = string.Empty;

    public ICommand LoginCommand { get; }

    public LoginViewModel(IUserService userService)
    {
        _userService = userService;
        LoginCommand = new LoginCommand(this);
    }

    public string Username
    {
        get => _username;
        set
        {
            _username = value;
            OnPropertyChanged();
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged();
        }
    }

    public async Task LoginAsync()
    {
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            MessageBox.Show("Please enter both username and password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        var result = await _userService.LoginAsync(Username, Password);

        if (result.IsSuccess)
        {
            var mainWindow = new MainWindow(result.Value, _userService);
            mainWindow.Show();

            foreach (Window window in System.Windows.Application.Current.Windows)
            {
                if (window is LoginWindow)
                {
                    window.Close();
                    break;
                }
            }
        }
        else
        {
            var errorMessage = result.Errors?.FirstOrDefault()?.Description ?? "Invalid username or password";
            MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
