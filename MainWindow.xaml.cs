using System.Windows;
using SimpleWpfMvvmAppV8.ViewModels;

namespace SimpleWpfMvvmAppV8;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }
}
