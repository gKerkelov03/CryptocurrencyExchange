using System;
using System.Linq;
using System.Threading.Tasks;
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
    private readonly IBalanceService _balanceService;
    private readonly ICryptoPriceService _cryptoPriceService;
    private string _username = string.Empty;
    private string _password = string.Empty;
    private string _errorMessage = string.Empty;
    private bool _isLoading;

    public LoginViewModel(IUserService userService, IBalanceService balanceService, ICryptoPriceService cryptoPriceService)
    {
        _userService = userService;
        _balanceService = balanceService;
        _cryptoPriceService = cryptoPriceService;
        LoginCommand = new LoginCommand(this);
    }

    public string Username
    {
        get => _username;
        set
        {
            _username = value;
            OnPropertyChanged();
            CommandManager.InvalidateRequerySuggested();
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged();
            CommandManager.InvalidateRequerySuggested();
        }
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            OnPropertyChanged();
        }
    }

    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            _isLoading = value;
            OnPropertyChanged();
            CommandManager.InvalidateRequerySuggested();
        }
    }

    public LoginCommand LoginCommand { get; }

    public async Task<bool> LoginAsync()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = string.Empty;

            var result = await _userService.LoginAsync(Username, Password);
            if (result.IsSuccess)
            {
                var mainWindow = new MainWindow(_userService, _balanceService, _cryptoPriceService, result.Value);
                mainWindow.Show();
                System.Windows.Application.Current.Windows[0].Close();
                return true;
            }
            else
            {
                ErrorMessage = result.Errors?.FirstOrDefault()?.Description ?? "Invalid username or password";
                return false;
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"An error occurred: {ex.Message}";
            return false;
        }
        finally
        {
            IsLoading = false;
        }
    }
}
