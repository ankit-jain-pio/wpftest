using System.Windows;
using System.Windows.Input;
using SimpleWpfMvvmAppV8.Commands;
using SimpleWpfMvvmAppV8.Services;
using SimpleWpfMvvmAppV8.Views;

namespace SimpleWpfMvvmAppV8.ViewModels;

public class MainWindowViewModel : BaseViewModel
{
    private BaseViewModel? _currentViewModel;
    private readonly INavigationService _navigationService;

    public BaseViewModel? CurrentViewModel
    {
        get => _currentViewModel;
        set => SetProperty(ref _currentViewModel, value);
    }

    public ICommand NavigateHomeCommand { get; }
    public ICommand NavigateUsersCommand { get; }
    public ICommand NavigateAddUserCommand { get; }
    public ICommand NavigateProductsCommand { get; }
    public ICommand NavigateAddProductCommand { get; }
    public ICommand NavigateAboutCommand { get; }
    public ICommand LogoutCommand { get; }

    public MainWindowViewModel()
    {
        // Create navigation service after this instance is created
        _navigationService = new NavigationService(this);
        
        NavigateHomeCommand = new RelayCommand(_ => NavigateToHome());
        NavigateUsersCommand = new RelayCommand(_ => NavigateToUsers());
        NavigateAddUserCommand = new RelayCommand(_ => NavigateToAddUser());
        NavigateProductsCommand = new RelayCommand(_ => NavigateToProducts());
        NavigateAddProductCommand = new RelayCommand(_ => NavigateToAddProduct());
        NavigateAboutCommand = new RelayCommand(_ => NavigateToAbout());
        LogoutCommand = new RelayCommand(_ => Logout());

        // Set initial view
        NavigateToHome();
    }

    public void NavigateToHome() => CurrentViewModel = new HomeViewModel();
    public void NavigateToUsers() => CurrentViewModel = new UsersViewModel();
    public void NavigateToAddUser() => CurrentViewModel = new AddUserViewModel();
    public void NavigateToProducts() => CurrentViewModel = new ProductsViewModel(_navigationService);
    public void NavigateToAddProduct() => CurrentViewModel = new AddEditProductViewModel(null, _navigationService);
    public void NavigateToEditProduct(int productId) => CurrentViewModel = new AddEditProductViewModel(productId, _navigationService);
    public void NavigateToAbout() => CurrentViewModel = new AboutViewModel();

    private static void Logout()
    {
        var loginWindow = new LoginWindow();
        loginWindow.Show();
        
        // Close the current main window
        Application.Current.MainWindow?.Close();
    }
}
