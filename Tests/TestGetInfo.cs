using Core;
using Moq;

namespace Tests;

public class Tests
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
    public void Test_GetCassettesInfo_Normal()
    {
        var mockCassettes = new List<CassetteModel>
        {
            new(10, 1, 100),
            new(50, 2, 99),
            new(100, 3, 98),
            new(500, 4, 97),
            new(1000, 5, 96),
            new(5000, 6, 95)
        };

        _atmRepositoryMock.Setup(repo => repo.GetCassettesInfo()).Returns(mockCassettes);
        
        var result = _atmService.GetCassettesInfo();

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(mockCassettes.Count));
        for (var i = 0; i < mockCassettes.Count; i++)
        {
            Assert.Multiple(() =>
            {
                Assert.That(result[i].DenominationValue, Is.EqualTo(mockCassettes[i].DenominationValue));
                Assert.That(result[i].CurrentCount, Is.EqualTo(mockCassettes[i].CurrentCount));
                Assert.That(result[i].Capacity, Is.EqualTo(mockCassettes[i].Capacity));
            });
        }
    }

    [Test]
    public void Test_GetCassettesInfo_Empty()
    {
        var mockCassettes = new List<CassetteModel>();
        
        _atmRepositoryMock.Setup(repo => repo.GetCassettesInfo()).Returns(mockCassettes);
        
        var result = _atmService.GetCassettesInfo();
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
    }
    
    [Test]
    public void Test_GetCassettesInfo_RepositoryThrowsException()
    {
        _atmRepositoryMock.Setup(repo => repo.GetCassettesInfo()).Throws(new Exception("Database error"));

        Assert.Throws<Exception>(() => _atmService.GetCassettesInfo());
    }
    
    
    [Test]
    public void Test_GetDenominationList_Normal()
    {
        var mockCassettes = new List<int> { 10, 50, 100, 500, 1000, 5000 };

        _atmRepositoryMock.Setup(repo => repo.GetDenominationList()).Returns(mockCassettes);
        
        var result = _atmService.GetDenominationList();

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(mockCassettes.Count));
        for (var i = 0; i < mockCassettes.Count; i++)
        {
            Assert.That(result[i], Is.EqualTo(mockCassettes[i]));
        }
    }
    
    [Test]
    public void Test_GetDenominationList_Empty()
    {
        var mockCassettes = new List<int>();
        
        _atmRepositoryMock.Setup(repo => repo.GetDenominationList()).Returns(mockCassettes);
        
        var result = _atmService.GetDenominationList();
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
    }
    
    
}