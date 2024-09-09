using System.Windows;
using System.Windows.Controls;
using ViewModel.ViewModel.Withdrawal;

namespace View;

public partial class PostWithdrawalPage : Page
{
    public PostWithdrawalPage(WithdrawalPageModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
    
    private void OnGetToStartPageClick(object sender, RoutedEventArgs e)
    {
        NavigationService!.Navigate(new StartPage());
    }
}