namespace SimpleWpfMvvmAppV8.Services;

public interface INavigationService
{
    void NavigateToHome();
    void NavigateToUsers();
    void NavigateToAddUser();
    void NavigateToProducts();
    void NavigateToAddProduct();
    void NavigateToEditProduct(int productId);
    void NavigateToAbout();
}
