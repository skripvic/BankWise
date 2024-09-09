using System.Windows;
using System.Windows.Controls;
using ViewModel.ViewModel.Deposit;

namespace View;

public partial class PostDepositPage : Page
{
    public PostDepositPage(DepositPageModel dataContext)
    {
        InitializeComponent();
        DataContext = dataContext;
    }
    
    private void OnGetToStartPageClick(object sender, RoutedEventArgs e)
    {
        NavigationService!.Navigate(new StartPage());
    }
}