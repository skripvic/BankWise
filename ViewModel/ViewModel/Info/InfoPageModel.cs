using System.Collections.ObjectModel;
using Core;
using ViewModel.ViewModel.Base;

namespace ViewModel.ViewModel.Info;

public class InfoPageModel : BaseViewModel
{
    private readonly IAtmService _atmService;
    private ObservableCollection<CassetteModel>? _cassettes;

    public ObservableCollection<CassetteModel> Cassettes
    {
        get => _cassettes;
        private set
        {
            _cassettes = value;
            OnPropertyChanged();
        }
    }

    public InfoPageModel(IAtmService atmService)
    {
        _atmService = atmService;
        LoadData();
    }

    private void LoadData()
    {
        var cassettes = _atmService.GetCassettesInfo();

        Cassettes = new ObservableCollection<CassetteModel>(cassettes);
    }
}