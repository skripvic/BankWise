namespace Core;

public interface IAtmService
{
    public List<CassetteModel> GetCassettesInfo();

    public List<int> GetDenominationList();

    public List<CassetteModel> DepositBanknotes(IEnumerable<CassetteModel> banknotes);

    public List<CassetteModel> WithdrawBanknotes(int sum, bool isExchange, int maxBanknoteExchangeCount);

    public bool CanWithdrawBanknotes(int sum);
    
    public int GetMinDenomination();
}