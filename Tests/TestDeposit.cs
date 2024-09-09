using Core;
using Moq;

namespace Tests;

public class TestDeposit
{
    private Mock<IAtmRepository> _atmRepositoryMock;
    private AtmService _atmService;
    
    [SetUp]
    public void Setup()
    {
        _atmRepositoryMock = new Mock<IAtmRepository>();
        _atmService = new AtmService(_atmRepositoryMock.Object);
    }
    
    [Test]
    public void Test_DepositBanknotes_AllFit()
    {
        var cassettes = new List<CassetteModel>
        {
            new CassetteModel (50,3,10),
            new CassetteModel (100,5,10)
        };
        var newBanknotes = new List<CassetteModel>
        {
            new CassetteModel (50,4),
            new CassetteModel (100,2)
        };

        _atmRepositoryMock.Setup(r => r.GetCassettesInfo()).Returns(cassettes);
        _atmRepositoryMock.Setup(r => r.UpdateCounts(It.Is<Dictionary<int, int>>(d => 
            d.Count == 2 && d[50] == 7 && d[100] == 7
            ))).Verifiable();
        
        var result = _atmService.DepositBanknotes(newBanknotes);
        
        Assert.That(result, Is.Empty);
        _atmRepositoryMock.Verify();
    }

    [Test] 
    public void Test_DepositBanknotes_PartFits()
    {
        var existingCassettes = new List<CassetteModel>
        {
            new(50,3,10),
            new(100,5,10),
            new(500,10,10)
        };
        var newBanknotes = new List<CassetteModel>
        {
            new(50,7,10),
            new(100,7,10),
            new(500,7,10)
        };

        _atmRepositoryMock.Setup(r => r.GetCassettesInfo()).Returns(existingCassettes);
        _atmRepositoryMock.Setup(r => r.UpdateCounts(It.Is<Dictionary<int, int>>(d => 
            d.Count == 3 && d[100] == 10 && d[50] == 10 && d[500] == 10
        ))).Verifiable();
        
        var result = _atmService.DepositBanknotes(newBanknotes);
        
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(result[0].DenominationValue, Is.EqualTo(100));
            Assert.That(result[0].CurrentCount, Is.EqualTo(2));
            Assert.That(result[1].DenominationValue, Is.EqualTo(500));
            Assert.That(result[1].CurrentCount, Is.EqualTo(7));
        });
        _atmRepositoryMock.Verify();
    }
    
    [Test] 
    public void Test_DepositBanknotes_NoneFit()
    {
        var existingCassettes = new List<CassetteModel>
        {
            new(50,0,10),
            new(100,1,10),
            new(500,10,10)
        };
        var newBanknotes = new List<CassetteModel>
        {
            new(50,20,10),
            new(100,19,10),
            new(500,10,10)
        };

        _atmRepositoryMock.Setup(r => r.GetCassettesInfo()).Returns(existingCassettes);
        _atmRepositoryMock.Setup(r => r.UpdateCounts(It.Is<Dictionary<int, int>>(d => 
            d.Count == 3 && d[100] == 10 && d[50] == 10 && d[500] == 10
        ))).Verifiable();
        
        var result = _atmService.DepositBanknotes(newBanknotes);
        
        Assert.That(result, Has.Count.EqualTo(3));
        Assert.Multiple(() =>
        {
            Assert.That(result[0].DenominationValue, Is.EqualTo(50));
            Assert.That(result[0].CurrentCount, Is.EqualTo(10));
            Assert.That(result[1].DenominationValue, Is.EqualTo(100));
            Assert.That(result[1].CurrentCount, Is.EqualTo(10));
            Assert.That(result[2].DenominationValue, Is.EqualTo(500));
            Assert.That(result[2].CurrentCount, Is.EqualTo(10));
        });
        _atmRepositoryMock.Verify();
    }
    
}