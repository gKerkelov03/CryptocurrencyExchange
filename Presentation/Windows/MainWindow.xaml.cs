using System.Windows;
using Application.Abstractions;
using Application.Domain;
using Presentation.ViewModels;

namespace Presentation.Windows;

public partial class MainWindow : Window
{
    private readonly IUserService _userService;
    private readonly IBalanceService _balanceService;
    private readonly ICryptoPriceService _cryptoPriceService;
    
    public MainWindow(IUserService userService, IBalanceService balanceService, ICryptoPriceService cryptoPriceService, User currentUser)
    {
        InitializeComponent();
        _userService = userService;
        _balanceService = balanceService;
        _cryptoPriceService = cryptoPriceService;
        DataContext = new MainViewModel(userService, balanceService, cryptoPriceService, currentUser);
    }
} 