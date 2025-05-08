using System;
using System.Windows;
using System.Windows.Input;
using Application.Abstractions;
using Application.Domain;

namespace Presentation.Commands;

public class SendCommand : ICommand
{
    private readonly IBalanceService _balanceService;
    private readonly User _currentUser;
    private readonly Action<Balance> _onBalanceSelected;
    private readonly Action<User> _onUserSelected;
    private readonly Action<double> _onAmountChanged;
    private readonly Action<string> _onError;
    private readonly Action _onSuccess;

    private Balance _selectedFromBalance;
    private User _selectedToUser;
    private double _transferAmount;

    public SendCommand(
        IBalanceService balanceService,
        User currentUser,
        Action<Balance> onBalanceSelected,
        Action<User> onUserSelected,
        Action<double> onAmountChanged,
        Action<string> onError,
        Action onSuccess)
    {
        _balanceService = balanceService;
        _currentUser = currentUser;
        _onBalanceSelected = onBalanceSelected;
        _onUserSelected = onUserSelected;
        _onAmountChanged = onAmountChanged;
        _onError = onError;
        _onSuccess = onSuccess;
    }

    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public void UpdateSelectedBalance(Balance balance)
    {
        _selectedFromBalance = balance;
        _onBalanceSelected?.Invoke(balance);
        CommandManager.InvalidateRequerySuggested();
    }

    public void UpdateSelectedUser(User user)
    {
        _selectedToUser = user;
        _onUserSelected?.Invoke(user);
        CommandManager.InvalidateRequerySuggested();
    }

    public void UpdateAmount(double amount)
    {
        _transferAmount = amount;
        _onAmountChanged?.Invoke(amount);
        CommandManager.InvalidateRequerySuggested();
    }

    public bool CanExecute(object parameter)
    {
        if (_selectedFromBalance == null || _selectedToUser == null)
            return false;

        if (_transferAmount <= 0)
            return false;

        if (_selectedFromBalance.Amount < _transferAmount)
            return false;

        return true;
    }

    public async void Execute(object parameter)
    {
        try
        {
            _onError?.Invoke(string.Empty);

            var transfer = new TransferRequest
            {
                FromUserId = _currentUser.Id,
                ToUserId = _selectedToUser.Id,
                CryptocurrencyId = _selectedFromBalance.CryptocurrencyId,
                Amount = _transferAmount
            };

            await _balanceService.TransferAsync(transfer);

            // Reset form
            _transferAmount = 0;
            _selectedFromBalance = null;
            _selectedToUser = null;
            _onAmountChanged?.Invoke(0);
            _onBalanceSelected?.Invoke(null);
            _onUserSelected?.Invoke(null);

            _onSuccess?.Invoke();
        }
        catch (Exception ex)
        {
            _onError?.Invoke($"Transfer failed: {ex.Message}");
        }
    }
} 