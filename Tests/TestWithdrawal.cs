using Core;
using Moq;

namespace Tests;

public class TestWithdrawal
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
    public void Test_WithdrawBanknotes_NormalNoExchange()
    {
        var existingCassettes = new Dictionary<int, int>()
        {
            [50] = 10,
            [100] = 10,
            [500] = 10,
            [1000] = 10
        };
        var expectedBanknotes = new List<CassetteModel>
        {
            new(50,1),
            new(100,3),
            new(1000,2)
        };

        _atmRepositoryMock.Setup(r => r.GetCassettesInfoAsDictionary()).Returns(existingCassettes);
        _atmRepositoryMock.Setup(r => r.UpdateCounts(It.Is<Dictionary<int, int>>(d => 
            d.Count == 4 && d[100] == 7 && d[50] == 9 && d[500] == 10 && d[1000] == 8
        ))).Verifiable();
        
        const int expectedSum = 2350;
        const bool isExchange = false;
        const int minBanknoteCount = 5;
        
        var result = _atmService.WithdrawBanknotes(expectedSum, isExchange, minBanknoteCount);
        
        var resultCheck = result
            .Select(b => (b.DenominationValue, b.CurrentCount))
            .ToList();
        Assert.That(result, Has.Count.EqualTo(3));
        Assert.Multiple(() =>
        {
            Assert.That(resultCheck, Does.Contain((expectedBanknotes[0].DenominationValue, expectedBanknotes[0].CurrentCount)));
            Assert.That(resultCheck, Does.Contain((expectedBanknotes[1].DenominationValue, expectedBanknotes[1].CurrentCount)));
            Assert.That(resultCheck, Does.Contain((expectedBanknotes[2].DenominationValue, expectedBanknotes[2].CurrentCount)));
        });
        _atmRepositoryMock.Verify();
    }
    
    [Test] 
    public void Test_WithdrawBanknotes_NormalWithExchange()
    {
        var existingCassettes = new Dictionary<int, int>()
        {
            [50] = 10,
            [100] = 10,
            [500] = 10
        };
        const int expectedSum = 2350;
        const bool isExchange = true;
        const int minBanknoteCount = 5;

        _atmRepositoryMock.Setup(r => r.GetCassettesInfoAsDictionary()).Returns(existingCassettes);
        _atmRepositoryMock.Setup(r => r.UpdateCounts(It.IsAny<Dictionary<int, int>>())).Verifiable();
        
        var result = _atmService.WithdrawBanknotes(expectedSum, isExchange, minBanknoteCount);
        
        Assert.That(result, Has.Count.EqualTo(3));
        Assert.That(result.Sum(elem => elem.DenominationValue * elem.CurrentCount), Is.EqualTo(expectedSum));
        Assert.That(result.Where(r => r.DenominationValue != 500).Select(r => r.CurrentCount), Is.All.AtLeast(minBanknoteCount));
        _atmRepositoryMock.Verify();
    }

    [Test]
    public void Test_WithdrawBanknotes_NotEnoughOfBanknotesWithExchange()
    {
        var existingCassettes = new Dictionary<int, int>()
        {
            [10] = 5,
            [50] = 3,
            [100] = 1
        };
        
        var expectedBanknotes = new List<CassetteModel>
        {
            new(10,1),
            new(50,3),
            new(100,1)
        };
        const int expectedSum = 260;
        const bool isExchange = true;
        const int minBanknoteCount = 5;

        _atmRepositoryMock.Setup(r => r.GetCassettesInfoAsDictionary()).Returns(existingCassettes);
        _atmRepositoryMock.Setup(r => r.UpdateCounts(It.IsAny<Dictionary<int, int>>())).Verifiable();
        
        var result = _atmService.WithdrawBanknotes(expectedSum, isExchange, minBanknoteCount);
        var resultCheck = result
            .Select(b => (b.DenominationValue, b.CurrentCount))
            .ToList();
        
        Assert.That(result, Has.Count.EqualTo(3));
        Assert.That(result.Sum(elem => elem.DenominationValue * elem.CurrentCount), Is.EqualTo(expectedSum));
        Assert.Multiple(() =>
        {
            Assert.That(resultCheck, Does.Contain((expectedBanknotes[0].DenominationValue, expectedBanknotes[0].CurrentCount)));
            Assert.That(resultCheck, Does.Contain((expectedBanknotes[1].DenominationValue, expectedBanknotes[1].CurrentCount)));
            Assert.That(resultCheck, Does.Contain((expectedBanknotes[2].DenominationValue, expectedBanknotes[2].CurrentCount)));
        });        
        _atmRepositoryMock.Verify();
    }
    
    [Test]
    public void Test_WithdrawBanknotes_NotEnoughOfBanknotesWithExchange2()
    {
        var existingCassettes = new Dictionary<int, int>()
        {
            [10] = 5,
            [50] = 3,
            [100] = 3,
            [500] = 2,
            [1000] = 1
        };
        
        var expectedBanknotes = new List<CassetteModel>
        {
            new(10,1),
            new(50,1),
            new(100,2),
            new(500,1),
            new(1000,1)
        };
        const int expectedSum = 1760;
        const bool isExchange = true;
        const int minBanknoteCount = 5;

        _atmRepositoryMock.Setup(r => r.GetCassettesInfoAsDictionary()).Returns(existingCassettes);
        _atmRepositoryMock.Setup(r => r.UpdateCounts(It.IsAny<Dictionary<int, int>>())).Verifiable();
        
        var result = _atmService.WithdrawBanknotes(expectedSum, isExchange, minBanknoteCount);
        var resultCheck = result
            .Select(b => (b.DenominationValue, b.CurrentCount))
            .ToList();
        
        Assert.That(result, Has.Count.EqualTo(expectedBanknotes.Count));
        Assert.That(result.Sum(elem => elem.DenominationValue * elem.CurrentCount), Is.EqualTo(expectedSum));
        Assert.Multiple(() =>
        {
            foreach (var expected in expectedBanknotes)
            {
                Assert.That(resultCheck, Does.Contain((expected.DenominationValue, expected.CurrentCount)));
            }
        });        
        _atmRepositoryMock.Verify();
    }
    
    [Test]
    public void Test_CanWithdrawBanknotes_Normal()
    {
        var cassettes = new List<CassetteModel>
        {
            new (10,  1 ),
            new ( 50, 10 ),
            new ( 100, 10 )
        };
        _atmRepositoryMock.Setup(r => r.GetCassettesInfo()).Returns(cassettes);

        const int sum = 250;
        
        var result = _atmService.CanWithdrawBanknotes(sum);

        Assert.IsTrue(result);
    }

    [Test]
    public void Test_CanWithdrawBanknotes_SumNotDivisible()
    {
        var cassettes = new List<CassetteModel>
        {
            new (10,  1 ),
            new ( 50, 1 ),
            new ( 100, 1 )
        };
        _atmRepositoryMock.Setup(r => r.GetCassettesInfo()).Returns(cassettes);

        const int sum = 55;

        var result = _atmService.CanWithdrawBanknotes(sum);

        Assert.IsFalse(result);
    }

    [Test]
    public void Test_CanWithdrawBanknotes_NotEnoughBanknotes()
    {
        var cassettes = new List<CassetteModel>
        {
            new (10,  1 ),
            new ( 50, 1 ),
            new ( 100, 1 )
        };
        _atmRepositoryMock.Setup(r => r.GetCassettesInfo()).Returns(cassettes);

        const int sum = 250;
        
        var result = _atmService.CanWithdrawBanknotes(sum);

        Assert.IsFalse(result);
    }

    [Test]
    public void Test_CanWithdrawBanknotes_ZeroSum()
    {
        var cassettes = new List<CassetteModel>
        {
            new(50, 10),
            new(100, 10)
        };
        _atmRepositoryMock.Setup(r => r.GetCassettesInfo()).Returns(cassettes);

        const int sum = 0;

        var result = _atmService.CanWithdrawBanknotes(sum);

        Assert.IsFalse(result);
    }
    
    [Test]
    public void Test_CanWithdrawBanknotes_AtmIsEmpty()
    {
        var cassettes = new List<CassetteModel>
        {
            new(50, 0),
            new(100, 0)
        };
        _atmRepositoryMock.Setup(r => r.GetCassettesInfo()).Returns(cassettes);

        const int sum = 150;

        var result = _atmService.CanWithdrawBanknotes(sum);

        Assert.IsFalse(result);
    }
    
    [Test]
    public void Test_CanWithdrawBanknotes_EmptyList()
    {
        var cassettes = new List<CassetteModel>();
        _atmRepositoryMock.Setup(r => r.GetCassettesInfo()).Returns(cassettes);

        const int sum = 150;

        var result = _atmService.CanWithdrawBanknotes(sum);

        Assert.IsFalse(result);
    }
    
}
