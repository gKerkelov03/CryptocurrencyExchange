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
    private readonly User _currentUser;

    public MainWindow(IUserService userService, IBalanceService balanceService, ICryptoPriceService cryptoPriceService, User currentUser)
    {
        InitializeComponent();
        _userService = userService;
        _balanceService = balanceService;
        _cryptoPriceService = cryptoPriceService;
        _currentUser = currentUser;
        DataContext = new MainViewModel(userService, balanceService, cryptoPriceService, currentUser);
    }

    private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
    {
        System.Windows.Application.Current.Shutdown();
    }

    private void LogoutMenuItem_Click(object sender, RoutedEventArgs e)
    {
        var loginWindow = new LoginWindow(_userService, _balanceService, _cryptoPriceService);
        loginWindow.Show();
        Close();
    }

    private void TradeButton_Click(object sender, RoutedEventArgs e)
    {
        // TODO: Implement trading functionality
        MessageBox.Show("Trading functionality will be implemented soon!", "Coming Soon", MessageBoxButton.OK, MessageBoxImage.Information);
    }
} 