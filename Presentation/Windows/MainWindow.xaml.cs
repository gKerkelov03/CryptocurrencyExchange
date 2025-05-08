using System.Windows;
using Application.Abstractions;
using Application.Domain;
using Presentation.ViewModels;

namespace Presentation.Windows;

public partial class MainWindow : Window
{
    private IUserService _userService;

    public MainWindow(User currentUser, IUserService userService)
    {
        InitializeComponent();
        _userService = userService;
        DataContext = new MainViewModel(currentUser, userService);
    }

    private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
    {
        System.Windows.Application.Current.Shutdown();
    }

    private void LogoutMenuItem_Click(object sender, RoutedEventArgs e)
    {
        var loginWindow = new LoginWindow(_userService);
        loginWindow.Show();
        Close();
    }

    private void TradeButton_Click(object sender, RoutedEventArgs e)
    {
        // TODO: Implement trading functionality
        MessageBox.Show("Trading functionality will be implemented soon!", "Coming Soon", MessageBoxButton.OK, MessageBoxImage.Information);
    }
} 