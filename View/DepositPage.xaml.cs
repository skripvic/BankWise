using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using Core;
using Microsoft.Extensions.DependencyInjection;
using ViewModel.ViewModel.Deposit;

namespace View;

public partial class DepositPage : Page
{
    public DepositPage()
    {
        InitializeComponent();
        DataContext = new DepositPageModel(App.ServiceProvider.GetRequiredService<IAtmService>());
    }
    
    private void OnAddBanknotesClick(object sender, RoutedEventArgs e)
    {
        var viewModel = DataContext as DepositPageModel;
        if (viewModel?.DepositBanknotesCommand.CanExecute(null) == true)
        {
            viewModel.DepositBanknotesCommand.Execute(null);
        }
        NavigationService!.Navigate(new PostDepositPage(viewModel!));
    }
    
    private void PreviewTextInput(object sender, TextChangedEventArgs e)
    {
        TextBox textBox = sender as TextBox;
        if (textBox == null)
            return;
        string text = textBox.Text;
        Regex regex = new Regex(@"^(200|[1-9]?[0-9]|1[0-9][0-9])$");
        if (!regex.IsMatch(text))
        {
            textBox.Text = "0";
            textBox.CaretIndex = 1; 
        }
    }
    
    private void OnGetBackClick(object sender, RoutedEventArgs e)
    {
        NavigationService!.GoBack();
    }
}