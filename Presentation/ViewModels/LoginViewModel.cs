using System.Windows;
using System.Windows.Input;
using Presentation.Commands;

namespace Presentation.ViewModels;

public class LoginViewModel : ViewModelBase
{
    private string _username = string.Empty;
    private string _password = string.Empty;

    public ICommand LoginCommand { get; }

    public LoginViewModel()
    {
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

       
            // var mainWindow = new MainWindow(_userService, user);
            // mainWindow.Show();
                    
                    
            //foreach (Window window in System.Windows.Application.Current.Windows)
            //{
            //    if (window is LoginWindow)
            //    {
            //        window.Close();
            //        break;
            //    }
            //}
                
            
            
           // MessageBox.Show("Invalid username or password", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        
    }
}
