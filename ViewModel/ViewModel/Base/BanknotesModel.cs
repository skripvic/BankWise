using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ViewModel.ViewModel.Base;

public class BanknotesModel : INotifyPropertyChanged
{
    public int DenominationValue { get; private set; }
    
    private int _count;
    public int Count
    {
        get => _count;
        set
        {
            if (_count == value) return;
            _count = value;
            OnPropertyChanged();
        }
    }

    public BanknotesModel(int denominationValue, int count)
    {
        DenominationValue = denominationValue;
        Count = count;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    
    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}