namespace Core;

public interface IAtmRepository
{
    public List<CassetteModel> GetCassettesInfo();
    
    public List<int> GetDenominationList();

    public void UpdateCounts(Dictionary<int, int> newCounts);
    
    public int GetMinDenomination();

    public Dictionary<int, int> GetCassettesInfoAsDictionary();
}