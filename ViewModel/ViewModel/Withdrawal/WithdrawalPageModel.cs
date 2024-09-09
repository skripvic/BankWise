using System.Collections.ObjectModel;
using Core;
using ViewModel.ViewModel.Base;

namespace ViewModel.ViewModel.Withdrawal;

public class WithdrawalPageModel : BaseViewModel
{
    private readonly IAtmService _atmService;
    private int _sum = 0;
    private int _min = 0;
    private const int MinBanknoteExchangeCount = 5;
    private bool _isExchange = false;
    public readonly RelayCommand GiveBanknotesCommand;

    public ObservableCollection<BanknotesModel> DeleteBanknotes { get; private set; } = new();
    
    public int Sum
    {
        get => _sum;
        set
        {
            _sum = value;
            OnPropertyChanged();
        }
    }
    
    public int Min
    {
        get => _min;
        private set
        {
            _min = value;
            OnPropertyChanged();
        }
    }
    
    public bool IsExchange
    {
        get => _isExchange;
        set
        {
            _isExchange = value;
            OnPropertyChanged();
        }
    }


    public WithdrawalPageModel(IAtmService atmService)
    {
        _atmService = atmService;
        GiveBanknotesCommand = new RelayCommand(WithdrawBanknotes, CanWithdrawBanknotes);
        GetMinDenomination();
    }

    private void GetMinDenomination()
    {
        Min = _atmService.GetMinDenomination();
    }

    private bool CanWithdrawBanknotes()
    {
        return _atmService.CanWithdrawBanknotes(Sum);
    }

    private void WithdrawBanknotes()
    {
        DeleteBanknotes = new ObservableCollection<BanknotesModel>();
        var result = _atmService.WithdrawBanknotes(Sum, IsExchange, MinBanknoteExchangeCount);
        foreach (var banknotes in result)
        {
            DeleteBanknotes.Add(new BanknotesModel(banknotes.DenominationValue, banknotes.CurrentCount));
        }
    }
}