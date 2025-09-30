using SimpleWpfMvvmAppV8.ViewModels;

namespace SimpleWpfMvvmAppV8.Services;

public class NavigationService(MainWindowViewModel mainWindowViewModel) : INavigationService
{
    private readonly MainWindowViewModel _mainWindowViewModel = mainWindowViewModel;

    public void NavigateToHome() => _mainWindowViewModel.NavigateToHome();
    public void NavigateToUsers() => _mainWindowViewModel.NavigateToUsers();
    public void NavigateToAddUser() => _mainWindowViewModel.NavigateToAddUser();
    public void NavigateToProducts() => _mainWindowViewModel.NavigateToProducts();
    public void NavigateToAddProduct() => _mainWindowViewModel.NavigateToAddProduct();
    public void NavigateToEditProduct(int productId) => _mainWindowViewModel.NavigateToEditProduct(productId);
    public void NavigateToAbout() => _mainWindowViewModel.NavigateToAbout();
}
