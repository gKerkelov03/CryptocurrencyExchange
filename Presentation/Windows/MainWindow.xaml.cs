using System.Windows;
using Application.Abstractions;
using Application.Domain;
using Presentation.ViewModels;

namespace Presentation.Windows;

public partial class MainWindow : Window
{
    private readonly IUserService _userService;
    private readonly IBalanceService _balanceService;
    private readonly ICurrencyPriceService _currencyPriceService;
    
    public MainWindow(IUserService userService, IBalanceService balanceService, ICurrencyPriceService currencyPriceService, User currentUser)
    {
        InitializeComponent();
        _userService = userService;
        _balanceService = balanceService;
        _currencyPriceService = currencyPriceService;
        DataContext = new MainViewModel(userService, balanceService, currencyPriceService, currentUser);
    }
} 