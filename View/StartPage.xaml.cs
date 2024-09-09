using System.Windows;
using System.Windows.Controls;

namespace View;

public partial class StartPage : Page
{
  
    public StartPage()
    {
        InitializeComponent();
    }
    
    private void OnGetInfoClick(object sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new InfoPage());
    }
    
    private void OnReceiveMoneyClick(object sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new DepositPage());
    }
    
    private void OnGiveMoneyClick(object sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new WithdrawalPage());
    }
}