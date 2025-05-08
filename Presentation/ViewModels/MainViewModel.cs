using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

public class MainViewModel : ViewModelBase
{
    private readonly IUserService _userService;
    private readonly IBalanceService _balanceService;
    private User _currentUser;
    private string _errorMessage;
    private Balance _selectedFromBalance;
    private User _selectedToUser;
    private double _transferAmount;
    private bool _isUpdating;

    public ICommand LogoutCommand { get; }
    public ICommand ExitCommand { get; }
    public ICommand SendCommand { get; }

    public MainViewModel(IUserService userService, IBalanceService balanceService, User currentUser)
    {
        _userService = userService;
        _balanceService = balanceService;
        _currentUser = currentUser;

        LogoutCommand = new LogoutCommand(userService, balanceService);
        ExitCommand = new ExitCommand();
        SendCommand = new SendCommand(
            balanceService,
            currentUser,
            balance => SelectedFromBalance = balance,
            user => SelectedToUser = user,
            amount => TransferAmount = amount,
            error => ErrorMessage = error,
            () => MessageBox.Show("Transfer completed successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information)
        );

        LoadData();
    }

    public ObservableCollection<Balance> NonZeroBalances { get; } = new();
    public ObservableCollection<User> OtherUsers { get; } = new();

    public Balance SelectedFromBalance
    {
        get => _selectedFromBalance;
        set
        {
            if (_isUpdating) return;
            _isUpdating = true;
            try
            {
                if (SetProperty(ref _selectedFromBalance, value))
                {
                    ((SendCommand)SendCommand).UpdateSelectedBalance(value);
                    CommandManager.InvalidateRequerySuggested();
                }
            }
            finally
            {
                _isUpdating = false;
            }
        }
    }

    public User SelectedToUser
    {
        get => _selectedToUser;
        set
        {
            if (_isUpdating) return;
            _isUpdating = true;
            try
            {
                if (SetProperty(ref _selectedToUser, value))
                {
                    ((SendCommand)SendCommand).UpdateSelectedUser(value);
                    CommandManager.InvalidateRequerySuggested();
                }
            }
            finally
            {
                _isUpdating = false;
            }
        }
    }

    public double TransferAmount
    {
        get => _transferAmount;
        set
        {
            if (_isUpdating) return;
            _isUpdating = true;
            try
            {
                if (SetProperty(ref _transferAmount, value))
                {
                    ((SendCommand)SendCommand).UpdateAmount(value);
                    CommandManager.InvalidateRequerySuggested();
                }
            }
            finally
            {
                _isUpdating = false;
            }
        }
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }


    private async void LoadData()
    {
        try
        {
            // Load non-zero balances
            var balances = await _balanceService.GetUserBalancesAsync(_currentUser.Id);
            NonZeroBalances.Clear();
            foreach (var balance in balances.Where(b => b.Amount > 0))
            {
                NonZeroBalances.Add(balance);
            }

            // Load other users
            var users = await _userService.GetAllUsersAsync();
            OtherUsers.Clear();
            foreach (var user in users.Where(u => u.Id != _currentUser.Id))
            {
                OtherUsers.Add(user);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error loading data: {ex.Message}";
        }
    }
} 