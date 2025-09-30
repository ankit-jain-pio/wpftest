using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using SimpleWpfMvvmAppV8.Commands;
using SimpleWpfMvvmAppV8.Models;
using SimpleWpfMvvmAppV8.Repositories;
using SimpleWpfMvvmAppV8.Services;

namespace SimpleWpfMvvmAppV8.ViewModels;

public class AddEditProductViewModel : BaseViewModel
{
    private readonly ProductRepository _repository = new();
    private readonly INavigationService? _navigationService;
    private readonly int? _productId;
    private string _name = string.Empty;
    private string _description = string.Empty;
    private decimal _price;
    private string _sku = string.Empty;
    private string _imagePath = string.Empty;
    private string _fullImagePath = string.Empty;
    private string _message = string.Empty;
    private Brush _messageForeground = Brushes.Black;

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    public decimal Price
    {
        get => _price;
        set => SetProperty(ref _price, value);
    }

    public string SKU
    {
        get => _sku;
        set => SetProperty(ref _sku, value);
    }

    public string ImagePath
    {
        get => _imagePath;
        set => SetProperty(ref _imagePath, value);
    }

    // This property provides the full path for image display
    public string FullImagePath
    {
        get => _fullImagePath;
        set => SetProperty(ref _fullImagePath, value);
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

    public string PageTitle => _productId.HasValue ? "Edit Product" : "Add Product";
    public string ButtonText => _productId.HasValue ? "Update Product" : "Add Product";

    public ICommand SaveProductCommand { get; }
    public ICommand BrowseImageCommand { get; }
    public ICommand BackToProductsCommand { get; }

    public AddEditProductViewModel(int? productId = null, INavigationService? navigationService = null)
    {
        _productId = productId;
        _navigationService = navigationService;
        
        SaveProductCommand = new RelayCommand(_ => SaveProduct());
        BrowseImageCommand = new RelayCommand(_ => BrowseImage());
        BackToProductsCommand = new RelayCommand(_ => BackToProducts());

        if (_productId.HasValue)
        {
            LoadProduct(_productId.Value);
        }
    }

    private void LoadProduct(int productId)
    {
        var product = _repository.GetProductById(productId);
        if (product != null)
        {
            Name = product.Name;
            Description = product.Description ?? string.Empty;
            Price = product.Price;
            SKU = product.SKU;
            ImagePath = product.ImagePath ?? string.Empty;
            
            // Set the full path for display
            if (!string.IsNullOrEmpty(ImagePath))
            {
                FullImagePath = GetFullImagePath(ImagePath);
            }
        }
    }

    private void SaveProduct()
    {
        if (ValidateProduct())
        {
            var product = new Product
            {
                Name = Name,
                Description = Description,
                Price = Price,
                SKU = SKU,
                ImagePath = ImagePath // Store relative path in database
            };

            if (_productId.HasValue)
            {
                product.Id = _productId.Value;
                _repository.UpdateProduct(product);
                Message = "Product updated successfully!";
            }
            else
            {
                _repository.AddProduct(product);
                Message = "Product added successfully!";
                ClearForm();
            }

            MessageForeground = Brushes.Green;
        }
    }

    private bool ValidateProduct()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            ShowError("Product name is required.");
            return false;
        }

        if (Price <= 0)
        {
            ShowError("Price must be greater than 0.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(SKU))
        {
            ShowError("SKU is required.");
            return false;
        }

        if (_repository.SKUExists(SKU, _productId))
        {
            ShowError("SKU already exists. Please use a different SKU.");
            return false;
        }

        return true;
    }

    private void ShowError(string errorMessage)
    {
        Message = errorMessage;
        MessageForeground = Brushes.Red;
    }

    private void BrowseImage()
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "Image files (*.jpg, *.jpeg, *.png, *.bmp)|*.jpg;*.jpeg;*.png;*.bmp|All files (*.*)|*.*",
            Title = "Select Product Image"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            try
            {
                // Get the project root directory (solution directory)
                var projectDir = GetProjectRootDirectory();
                var imagesDir = Path.Combine(projectDir, "ProductImages");
                Directory.CreateDirectory(imagesDir);

                // Generate unique filename using SKU (or temp ID if SKU is empty)
                var skuForFile = !string.IsNullOrEmpty(SKU) ? SKU : "TEMP";
                var fileExtension = Path.GetExtension(openFileDialog.FileName);
                var fileName = $"{skuForFile}_{Guid.NewGuid().ToString("N")[..8]}{fileExtension}";
                var destinationPath = Path.Combine(imagesDir, fileName);

                // Copy file to ProductImages folder
                File.Copy(openFileDialog.FileName, destinationPath, true);

                // Store relative path in ImagePath (for database)
                ImagePath = Path.Combine("ProductImages", fileName);
                
                // Store full path for display
                FullImagePath = destinationPath;
                
                Message = "Image uploaded successfully!";
                MessageForeground = Brushes.Green;
            }
            catch (Exception ex)
            {
                ShowError($"Failed to upload image: {ex.Message}");
            }
        }
    }

    private static string GetProjectRootDirectory()
    {
        // Get the executing assembly location
        var assemblyLocation = Assembly.GetExecutingAssembly().Location;
        var assemblyDir = Path.GetDirectoryName(assemblyLocation);
        
        // Navigate up to find the project root (where .csproj file is located)
        var currentDir = new DirectoryInfo(assemblyDir!);
        while (currentDir != null && !File.Exists(Path.Combine(currentDir.FullName, "SimpleWpfMvvmAppV8.csproj")))
        {
            currentDir = currentDir.Parent;
        }
        
        return currentDir?.FullName ?? Directory.GetCurrentDirectory();
    }

    private static string GetFullImagePath(string relativePath)
    {
        if (string.IsNullOrEmpty(relativePath))
            return string.Empty;
            
        var projectDir = GetProjectRootDirectory();
        var fullPath = Path.Combine(projectDir, relativePath);
        
        return File.Exists(fullPath) ? fullPath : string.Empty;
    }

    private void BackToProducts()
    {
        if (_navigationService != null)
        {
            _navigationService.NavigateToProducts();
        }
        else
        {
            // Fallback to the old method if no navigation service is available
            var mainViewModel = Application.Current.MainWindow?.DataContext as MainWindowViewModel;
            mainViewModel?.NavigateToProducts();
        }
    }

    private void ClearForm()
    {
        Name = string.Empty;
        Description = string.Empty;
        Price = 0;
        SKU = string.Empty;
        ImagePath = string.Empty;
        FullImagePath = string.Empty;
    }
}
