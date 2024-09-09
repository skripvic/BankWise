using Core;
using Data.Database;
using Microsoft.EntityFrameworkCore;

namespace Data.DataAccess;

public class AtmRepository : IAtmRepository
{
    private readonly AppDbContext _context;

    public AtmRepository(AppDbContext context)
    {
        _context = context;
    }

    public List<CassetteModel> GetCassettesInfo()
    {
        return _context.Cassettes
            .Include(c => c.Denomination)
            .Select(cassette => new CassetteModel(cassette.Denomination.Value, cassette.CurrentCount, cassette.Capacity))
            .ToList();
        
    }

    public List<int> GetDenominationList()
    {
        return _context.Denominations
            .Select(d => d.Value)
            .ToList();
    }

    public void UpdateCounts(Dictionary<int, int> newCounts)
    {
        var cassette = _context.Cassettes
            .Include(c => c.Denomination)
            .ToList();

        foreach (var newCount in newCounts)
        {
            cassette
                .First(c => c.Denomination.Value == newCount.Key)
                .CurrentCount = newCount.Value;
        }
        
        _context.SaveChanges();
    }

    public int GetMinDenomination()
    {
        return _context.Cassettes
            .Where(c => c.CurrentCount > 0)
            .OrderBy(c => c.Denomination.Value)
            .Select(c => c.Denomination.Value)
            .First();
    }
    
    public Dictionary<int, int> GetCassettesInfoAsDictionary()
    {
        return _context.Cassettes
            .Include(c => c.Denomination)
            .ToDictionary(cassette => cassette.Denomination.Value, cassette => cassette.CurrentCount);
    }
}