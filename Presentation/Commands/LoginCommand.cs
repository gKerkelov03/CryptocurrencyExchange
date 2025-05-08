using System.Windows.Input;
using Presentation.ViewModels;

namespace Presentation.Commands;

public class LoginCommand : ICommand
{
    private readonly LoginViewModel _viewModel;
    private bool _isExecuting;

    public LoginCommand(LoginViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
        return !_isExecuting;
    }

    public async void Execute(object? parameter)
    {
        if (_isExecuting)
            return;

        try
        {
            _isExecuting = true;
            RaiseCanExecuteChanged();
            
            await _viewModel.LoginAsync();
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show($"Error during login: {ex.Message}", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
        }
        finally
        {
            _isExecuting = false;
            RaiseCanExecuteChanged();
        }
    }

    private void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
