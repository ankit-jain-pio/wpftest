namespace SimpleWpfMvvmAppV8.ViewModels;

public class AboutViewModel : BaseViewModel
{
    public string ApplicationName { get; } = "Simple WPF Application";
    public string Version { get; } = "1.0.0";
    public string Description { get; } = "A simple WPF application demonstrating MVVM pattern.";
}
