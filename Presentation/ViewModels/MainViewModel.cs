using System.Collections.ObjectModel;
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
    private readonly User _currentUser;
    private readonly IUserService _userService;
    private ObservableCollection<Balance> _balances;

    public ICommand LogoutCommand { get; }
    public ICommand TradeCommand { get; }
    public ICommand ExitCommand { get; }

    public MainViewModel(User currentUser, IUserService userService)
    {
        _currentUser = currentUser;
        _userService = userService;
        
        LogoutCommand = new LogoutCommand(userService);
        TradeCommand = new TradeCommand();
        ExitCommand = new ExitCommand();

        LoadUserData();
    }

    public ObservableCollection<Balance> Balances
    {
        get => _balances;
        set
        {
            _balances = value;
            OnPropertyChanged();
        }
    }

    public string Username => _currentUser.Username;

    private void LoadUserData()
    {
        if (_currentUser.Balances != null)
        {
            Balances = new ObservableCollection<Balance>(_currentUser.Balances);
        }
    }
} 