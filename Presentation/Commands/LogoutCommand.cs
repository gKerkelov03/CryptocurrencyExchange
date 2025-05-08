using System.Windows;
using System.Windows.Input;
using Application.Abstractions;
using Presentation.Windows;

namespace Presentation.Commands;

public class LogoutCommand : ICommand
{
    private readonly IUserService _userService;

    public LogoutCommand(IUserService userService)
    {
        _userService = userService;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter)
    {
        var loginWindow = new LoginWindow(_userService);
        loginWindow.Show();
        
        if (parameter is Window window)
        {
            window.Close();
        }
    }
} 