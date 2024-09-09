using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Core;
using ViewModel.ViewModel.Base;
using ViewModel.ViewModel.Withdrawal;

namespace ViewModel.ViewModel.Deposit;

public class DepositPageModel : BaseViewModel
{
    private readonly IAtmService _atmService;
    private int _sum = 0;
    private bool _isReturnBanknotesEmpty = true;
    public readonly RelayCommand DepositBanknotesCommand;
    
    public BindingList<BanknotesModel> Banknotes { get; private set; } = new();
    public ObservableCollection<CassetteModel>? ReturnBanknotes { get; private set; }
    public int Sum
    {
        get => _sum;
        set
        {
            _sum = value;
            OnPropertyChanged();
        }
    }
    public bool IsReturnBanknotesEmpty
    {
        get => _isReturnBanknotesEmpty;
        private set
        {
            _isReturnBanknotesEmpty = value;
            OnPropertyChanged();
        }
    }

    public DepositPageModel(IAtmService atmService)
    {
        _atmService = atmService;
        LoadDenominationList();
        Banknotes.ListChanged += ChangeSum!;
        DepositBanknotesCommand = new RelayCommand(DepositBanknotes);
    }
    
    
    private void ChangeSum(object sender, ListChangedEventArgs e)
    {
        if (e.ListChangedType != ListChangedType.ItemChanged) return;
        Sum = Banknotes.Sum(banknote => banknote.DenominationValue * banknote.Count);
    }

    private void LoadDenominationList()
    {
        var denominations = _atmService.GetDenominationList();
        Banknotes = new BindingList<BanknotesModel>();
        foreach (var denominationValue in denominations)
        {
            Banknotes.Add(new BanknotesModel(denominationValue, 0));
        }
    }

    private void DepositBanknotes()
    {
        var depositBanknotes = Banknotes.Select(banknote => new CassetteModel(banknote.DenominationValue, banknote.Count)).ToList();
        ReturnBanknotes = new ObservableCollection<CassetteModel>(_atmService.DepositBanknotes(depositBanknotes));
        if (ReturnBanknotes.Count != 0) IsReturnBanknotesEmpty = false;
    }
}