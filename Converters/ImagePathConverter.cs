using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Data;

namespace SimpleWpfMvvmAppV8.Converters;

public class ImagePathConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string relativePath && !string.IsNullOrEmpty(relativePath))
        {
            // Get the project root directory
            var projectDir = GetProjectRootDirectory();
            var fullPath = Path.Combine(projectDir, relativePath);
            
            return File.Exists(fullPath) ? fullPath : null;
        }
        
        return null;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
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
}
