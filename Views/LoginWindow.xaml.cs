using System.Windows;
using SimpleWpfMvvmAppV8.ViewModels;

namespace SimpleWpfMvvmAppV8.Views
{
    public partial class LoginWindow : Window
    {
        private LoginViewModel _viewModel;

        public LoginWindow()
        {
            InitializeComponent();
            _viewModel = new LoginViewModel();
            _viewModel.LoginSuccessful += OnLoginSuccessful;
            DataContext = _viewModel;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.Password = PasswordBox.Password;
            }
        }

        private void OnLoginSuccessful(object sender, EventArgs e)
        {
            var mainWindow = new SimpleWpfMvvmAppV8.MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}