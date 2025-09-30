using System.Windows.Input;
using System.Windows.Media;
using SimpleWpfMvvmAppV8.Commands;
using SimpleWpfMvvmAppV8.Models;
using SimpleWpfMvvmAppV8.Repositories;

namespace SimpleWpfMvvmAppV8.ViewModels;

public class AddUserViewModel : BaseViewModel
{
    private readonly UserRepository _repository = new();
    private string _name = string.Empty;
    private string _email = string.Empty;
    private string _message = string.Empty;
    private Brush _messageForeground = Brushes.Black;

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    public string Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }

    public Brush MessageForeground
    {
        get => _messageForeground;
        set => SetProperty(ref _messageForeground, value);
    }

    public ICommand AddUserCommand { get; }

    public AddUserViewModel()
    {
        AddUserCommand = new RelayCommand(_ => AddUser());
    }

    private void AddUser()
    {
        if (!string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Email))
        {
            var user = new User { Name = Name, Email = Email };
            _repository.AddUser(user);

            Message = $"User '{Name}' added.";
            MessageForeground = Brushes.Green;
            
            // Clear form
            Name = string.Empty;
            Email = string.Empty;
        }
        else
        {
            Message = "All fields required.";
            MessageForeground = Brushes.Red;
        }
    }
}
