using System.Windows.Input;
using SimpleWpfMvvmAppV8.Commands;

namespace SimpleWpfMvvmAppV8.ViewModels;

public class LoginViewModel : BaseViewModel
{
    private const string ValidUsername = "ankit";
    private const string ValidPassword = "password@123";

    private string _username = string.Empty;
    private string _password = string.Empty;
    private string _errorMessage = string.Empty;
    private bool _hasError;

    public string Username
    {
        get => _username;
        set => SetProperty(ref _username, value);
    }

    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    public bool HasError
    {
        get => _hasError;
        set => SetProperty(ref _hasError, value);
    }

    public ICommand LoginCommand { get; }

    public event EventHandler? LoginSuccessful;

    public LoginViewModel()
    {
        LoginCommand = new RelayCommand(_ => Login(), _ => CanLogin());
    }

    private bool CanLogin()
    {
        return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
    }

    private void Login()
    {
        if (Username == ValidUsername && Password == ValidPassword)
        {
            HasError = false;
            ErrorMessage = string.Empty;
            LoginSuccessful?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            HasError = true;
            ErrorMessage = "Invalid username or password. Please try again.";
        }
    }
}
