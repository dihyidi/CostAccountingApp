namespace CostAccounting.Application.Services.Interfaces;

public interface ICostAccountingService
{
    int GetRemainingSharesNumberAfterSale(int amountToSale);

    double GetCostBasisOfSoldShares(int amountToSale);
    
    double GetCostBasisOfRemainingShares(int amountToSale);

    double GetProfitOnSale(int amountToSale, double priceForSale);
}