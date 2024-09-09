namespace Core;

public class AtmService : IAtmService
{
    private readonly IAtmRepository _atmRepository;


    public AtmService(IAtmRepository atmRepository)
    {
        _atmRepository = atmRepository;
    }

    public List<CassetteModel> GetCassettesInfo()
    {
        return _atmRepository.GetCassettesInfo();
    }

    public List<int> GetDenominationList()
    {
        return _atmRepository.GetDenominationList();
    }


    public List<CassetteModel> DepositBanknotes(IEnumerable<CassetteModel> banknotes)
    {
        var cassettes = _atmRepository.GetCassettesInfo();

        var returnBanknotes = new List<CassetteModel>();
        var newCounts = new Dictionary<int, int>();

        foreach (var banknote in banknotes)
        {
            var cassette = cassettes.First(c => c.DenominationValue == banknote.DenominationValue);
            var count = cassette.CurrentCount + banknote.CurrentCount;

            if (count > cassette.Capacity)
            {
                newCounts.Add(banknote.DenominationValue, (int)cassette.Capacity);
                var excess = count - (int)cassette.Capacity;
                returnBanknotes.Add(new CassetteModel(banknote.DenominationValue, excess));
            }
            else
            {
                newCounts.Add(banknote.DenominationValue, count);
            }
        }

        _atmRepository.UpdateCounts(newCounts);

        return returnBanknotes;
    }

    public List<CassetteModel> WithdrawBanknotes(int sum, bool isExchange, int maxBanknoteExchangeCount)
    {
        return isExchange
            ? WithdrawBanknotesWithExchange(sum, maxBanknoteExchangeCount)
            : WithdrawBanknotesNoExchange(sum);
    }

    private List<CassetteModel> WithdrawBanknotesWithExchange(int sum, int maxBanknoteExchangeCount)
    {
        var cassettes = _atmRepository.GetCassettesInfoAsDictionary();
        var deleteBanknotes = new Dictionary<int, int>();
        var remainder = sum;

        var sortedCassettes = ToAscendingDictionary(cassettes);
        GetBanknotesBySum(ref remainder, sortedCassettes, deleteBanknotes, maxBanknoteExchangeCount);

        sortedCassettes = ToDescendingDictionary(sortedCassettes);
        GetBanknotesBySum(ref remainder, sortedCassettes, deleteBanknotes);

        if (remainder != 0) return HandleRemainder(remainder, sum, deleteBanknotes, sortedCassettes);

        _atmRepository.UpdateCounts(sortedCassettes);
        return DictionaryToList(deleteBanknotes);
    }

    private List<CassetteModel> HandleRemainder(int remainder, int sum, Dictionary<int, int> deleteBanknotes,
        Dictionary<int, int> sortedCassettes)
    {
        var sumToGiveBack = remainder;
        sortedCassettes = ToAscendingDictionary(sortedCassettes);
        
        var cassette = sortedCassettes
            .FirstOrDefault(cassette => remainder < cassette.Key && cassette.Value > 0);
        
        if (!cassette.Equals(default(KeyValuePair<int, int>)))
        {
            sumToGiveBack = cassette.Key - remainder;
            deleteBanknotes = ToAscendingDictionary(deleteBanknotes);

            GetBanknotesBySum(ref sumToGiveBack, deleteBanknotes, sortedCassettes);

            deleteBanknotes[cassette.Key] =
                deleteBanknotes.TryGetValue(cassette.Key, out var banknoteCount)
                    ? banknoteCount + 1
                    : 1;
        }

        if (sumToGiveBack > 0)
        {
            return WithdrawBanknotesNoExchange(sum);
        }

        _atmRepository.UpdateCounts(sortedCassettes);
        return DictionaryToList(deleteBanknotes);
    }

    private List<CassetteModel> WithdrawBanknotesNoExchange(int sum)
    {
        var cassettes = _atmRepository.GetCassettesInfoAsDictionary();
        var deleteBanknotes = new Dictionary<int, int>();

        var sortedCassettes = ToDescendingDictionary(cassettes);
        GetBanknotesBySum(ref sum, sortedCassettes, deleteBanknotes);

        _atmRepository.UpdateCounts(sortedCassettes);
        return DictionaryToList(deleteBanknotes);
    }

    private static List<CassetteModel> DictionaryToList(Dictionary<int, int> dictionary)
    {
        return dictionary
            .Select(b => new CassetteModel(b.Key, b.Value))
            .ToList();
    }

    private static Dictionary<int, int> ToAscendingDictionary(Dictionary<int, int> dictionary)
    {
        return dictionary
            .OrderBy(c => c.Key)
            .ToDictionary(c => c.Key, c => c.Value);
    }

    private static Dictionary<int, int> ToDescendingDictionary(Dictionary<int, int> dictionary)
    {
        return dictionary
            .OrderByDescending(c => c.Key)
            .ToDictionary(c => c.Key, c => c.Value);
    }

    private static void GetBanknotesBySum(ref int sum, Dictionary<int, int> inBanknotes,
        Dictionary<int, int> outBanknotes,
        int maxBanknoteCount = int.MaxValue)
    {
        foreach (var banknote in inBanknotes)
        {
            if (sum <= 0) break;
            var banknotesCount = sum / banknote.Key;

            banknotesCount = Math.Min(Math.Min(banknotesCount, maxBanknoteCount), banknote.Value);

            if (banknotesCount <= 0) continue;

            outBanknotes[banknote.Key] =
                outBanknotes.TryGetValue(banknote.Key, out var count)
                    ? count + banknotesCount
                    : banknotesCount;
            sum -= banknotesCount * banknote.Key;
            inBanknotes[banknote.Key] -= banknotesCount;
        }
    }

    public bool CanWithdrawBanknotes(int sum)
    {
        var cassettes = _atmRepository.GetCassettesInfo()
            .OrderByDescending(c => c.DenominationValue)
            .ToList();
        if (cassettes.Count == 0)
        {
            return false;
        }
        
        if (sum % cassettes.Last().DenominationValue != 0 || !cassettes.Any(c => c.CurrentCount > 0))
        {
            return false;
        }

        foreach (var cassette in cassettes)
        {
            var banknotesCount = sum / cassette.DenominationValue;
            if (banknotesCount <= 0) continue;

            if (banknotesCount <= cassette.CurrentCount)
            {
                sum -= banknotesCount * cassette.DenominationValue;
            }
            else
            {
                sum -= cassette.CurrentCount * cassette.DenominationValue;
            }

            if (sum == 0)
            {
                return true;
            }
        }

        return false;
    }

    public int GetMinDenomination()
    {
        return _atmRepository.GetMinDenomination();
    }
}