using System.Collections.ObjectModel;
using SimpleWpfMvvmAppV8.Models;
using SimpleWpfMvvmAppV8.Repositories;

namespace SimpleWpfMvvmAppV8.ViewModels;

public class UsersViewModel : BaseViewModel
{
    private readonly UserRepository _repository = new();
    private ObservableCollection<User> _users = [];

    public ObservableCollection<User> Users
    {
        get => _users;
        set => SetProperty(ref _users, value);
    }

    public UsersViewModel()
    {
        LoadUsers();
    }

    private void LoadUsers()
    {
        var userList = _repository.GetAllUsers();
        Users = new ObservableCollection<User>(userList);
    }
}
