using System.Windows.Input;
using Presentation.ViewModels;

namespace Presentation.Commands;

public class LoginCommand : ICommand
{
    private readonly LoginViewModel _viewModel;

    public LoginCommand(LoginViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter)
    {
        try
        {
            _viewModel.LoginAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show($"Error during login: {ex.Message}", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
        }
    }
}
