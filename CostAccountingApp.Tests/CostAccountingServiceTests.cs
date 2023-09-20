using CostAccounting.Application.Services;
using CostAccounting.Application.Services.Interfaces;
using CostAccounting.Data.Entities;
using CostAccounting.Data.Repositories.Interfaces;
using Moq;
using Xunit;

namespace CostAccountingApp.Tests;

public class CostAccountingServiceTests
{
    private const int OwnedAmount = 190;
    private static readonly List<PurchasedShareEntity> OwnedShares = new()
    {
        new() { Id = 1, Amount = 120, Price = 20 },
        new() { Id = 2, Amount = 70, Price = 30 }
    };
    
    private readonly ICostAccountingService costAccountingService;
    private readonly Mock<IShareRepository> shareRepositoryMock;

    public CostAccountingServiceTests()
    {
        shareRepositoryMock = new Mock<IShareRepository>();
        costAccountingService = new CostAccountingService(shareRepositoryMock.Object);
    }

    #region GetRemainingSharesNumberAfterSale

    [Fact]
    public void GetRemainingSharesNumberAfterSale_AmountToSaleIsMoreThanOwned_ThrowsException()
    {
        //arrange
        var owned = 100;
        var amountToSale = 150;
        shareRepositoryMock.Setup(x => x.GetSharesAmount()).Returns(owned);

        //act

        //assert
        Assert.Throws<Exception>(() => costAccountingService.GetRemainingSharesNumberAfterSale(amountToSale));
    }
    
    [Fact]
    public void GetRemainingSharesNumberAfterSale_ValidParams_ShouldReturnCorrectAmount()
    {
        //arrange
        var amountToSale = 100;
        var expected = 90;
        shareRepositoryMock.Setup(x => x.GetSharesAmount()).Returns(OwnedAmount);

        //act
        var actual = costAccountingService.GetRemainingSharesNumberAfterSale(amountToSale);

        //assert
        Assert.Equal(expected, actual);
    }

    #endregion

    #region GetCostBasisOfSoldShares
    
    [Fact]
    public void GetCostBasisOfSoldShares_AmountToSaleIsMoreThanOwned_ThrowsException()
    {
        //arrange
        var owned = 100;
        var amountToSale = 150;
        shareRepositoryMock.Setup(x => x.GetSharesAmount()).Returns(owned);

        //act

        //assert
        Assert.Throws<Exception>(() => costAccountingService.GetRemainingSharesNumberAfterSale(amountToSale));
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(120, 20)]
    [InlineData(150, 22)]
    [InlineData(190, 23.684)]
    public void GetCostBasisOfSoldShares_ValidData_ReturnsCorrectPrice(int amountToSale, double expectedPrice)
    {
        // arrange
        shareRepositoryMock.Setup(x => x.GetSharesAmount()).Returns(OwnedAmount);
        shareRepositoryMock.Setup(x => x.GetSharesBalance()).Returns(OwnedShares);

        // act
        var actualPrice = costAccountingService.GetCostBasisOfSoldShares(amountToSale);

        // assert
        Assert.Equal(expectedPrice, actualPrice, 1e-3);
    }

    #endregion
    
    #region GetCostBasisOfRemainingShares
    
    [Fact]
    public void GetCostBasisOfRemainingShares_AmountToSaleIsMoreThanOwned_ThrowsException()
    {
        //arrange
        var owned = 100;
        var amountToSale = 150;
        shareRepositoryMock.Setup(x => x.GetSharesAmount()).Returns(owned);

        //act

        //assert
        Assert.Throws<Exception>(() => costAccountingService.GetRemainingSharesNumberAfterSale(amountToSale));
    }

    [Theory]
    [InlineData(0, 23.684)]
    [InlineData(100, 27.777)]
    [InlineData(120, 30)]
    [InlineData(190, 0)]
    public void GetCostBasisOfRemainingShares_ValidData_ReturnsCorrectPrice(int amountToSale, double expectedPrice)
    {
        // arrange
        shareRepositoryMock.Setup(x => x.GetSharesAmount()).Returns(OwnedAmount);
        shareRepositoryMock.Setup(x => x.GetSharesBalance()).Returns(OwnedShares);

        // act
        var actualPrice = costAccountingService.GetCostBasisOfRemainingShares(amountToSale);

        // assert
        Assert.Equal(expectedPrice, actualPrice, 1e-3);
    }

    #endregion
    
    #region GetProfitOnSale
    
    [Fact]
    public void GetProfitOnSale_AmountToSaleIsMoreThanOwned_ThrowsException()
    {
        //arrange
        var owned = 100;
        var amountToSale = 150;
        shareRepositoryMock.Setup(x => x.GetSharesAmount()).Returns(owned);

        //act

        //assert
        Assert.Throws<Exception>(() => costAccountingService.GetRemainingSharesNumberAfterSale(amountToSale));
    }

    [Theory]
    [InlineData(0, 110, 0)]
    [InlineData(100, 40, 2000)]
    [InlineData(100, 10, -1000)]
    [InlineData(150, 40, 2700)]
    [InlineData(150, 30, 1200)]
    [InlineData(150, 20, -300)]
    [InlineData(150, 10, -1800)]
    public void GetProfitOnSale_ValidData_AllPossiblePaths_ReturnsCorrectPrice(int amountToSale, double priceForSale, double expectedProfit)
    {
        // arrange
        shareRepositoryMock.Setup(x => x.GetSharesAmount()).Returns(OwnedAmount);
        shareRepositoryMock.Setup(x => x.GetSharesBalance()).Returns(OwnedShares);

        // act
        var actualProfit = costAccountingService.GetProfitOnSale(amountToSale, priceForSale);

        // assert
        Assert.Equal(expectedProfit, actualProfit, 1e-3);
    }

    #endregion
}