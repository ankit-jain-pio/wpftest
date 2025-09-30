using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using SimpleWpfMvvmAppV8.Commands;
using SimpleWpfMvvmAppV8.Models;
using SimpleWpfMvvmAppV8.Repositories;
using SimpleWpfMvvmAppV8.Services;

namespace SimpleWpfMvvmAppV8.ViewModels;

public class ProductsViewModel : BaseViewModel
{
    private readonly ProductRepository _repository = new();
    private readonly INavigationService? _navigationService;
    private ObservableCollection<Product> _products = [];
    private Product? _selectedProduct;

    public ObservableCollection<Product> Products
    {
        get => _products;
        set => SetProperty(ref _products, value);
    }

    public Product? SelectedProduct
    {
        get => _selectedProduct;
        set
        {
            SetProperty(ref _selectedProduct, value);
            ((RelayCommand)EditProductCommand).RaiseCanExecuteChanged();
            ((RelayCommand)DeleteProductCommand).RaiseCanExecuteChanged();
        }
    }

    public ICommand AddNewProductCommand { get; }
    public ICommand EditProductCommand { get; }
    public ICommand DeleteProductCommand { get; }
    public ICommand RefreshCommand { get; }

    public ProductsViewModel(INavigationService? navigationService = null)
    {
        _navigationService = navigationService;
        
        AddNewProductCommand = new RelayCommand(_ => AddNewProduct());
        EditProductCommand = new RelayCommand(EditProduct, CanExecuteProductCommand);
        DeleteProductCommand = new RelayCommand(DeleteProduct, CanExecuteProductCommand);
        RefreshCommand = new RelayCommand(_ => LoadProducts());

        LoadProducts();
    }

    private bool CanExecuteProductCommand(object? parameter)
    {
        return parameter is Product || SelectedProduct != null;
    }

    private void LoadProducts()
    {
        var productList = _repository.GetAllProducts();
        Products = new ObservableCollection<Product>(productList);
    }

    private void AddNewProduct()
    {
        if (_navigationService != null)
        {
            _navigationService.NavigateToAddProduct();
        }
        else
        {
            // Fallback to the old method if no navigation service is available
            var mainViewModel = Application.Current.MainWindow?.DataContext as MainWindowViewModel;
            mainViewModel?.NavigateToAddProduct();
        }
    }

    private void EditProduct(object? parameter)
    {
        var product = parameter as Product ?? SelectedProduct;
        if (product != null)
        {
            if (_navigationService != null)
            {
                _navigationService.NavigateToEditProduct(product.Id);
            }
            else
            {
                // Fallback to the old method if no navigation service is available
                var mainViewModel = Application.Current.MainWindow?.DataContext as MainWindowViewModel;
                mainViewModel?.NavigateToEditProduct(product.Id);
            }
        }
    }

    private void DeleteProduct(object? parameter)
    {
        var product = parameter as Product ?? SelectedProduct;
        if (product != null)
        {
            var result = MessageBox.Show(
                $"Are you sure you want to delete the product '{product.Name}'?\n\nThis action cannot be undone.",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning,
                MessageBoxResult.No);

            if (result == MessageBoxResult.Yes)
            {
                _repository.DeleteProduct(product.Id);
                LoadProducts(); // Refresh the list
                
                MessageBox.Show(
                    $"Product '{product.Name}' has been deleted successfully.",
                    "Product Deleted",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }
    }
}
