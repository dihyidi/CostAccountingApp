using CostAccounting.Application.Models;
using CostAccounting.Application.Services.Interfaces;
using CostAccounting.Data.Entities;
using CostAccounting.Data.Repositories.Interfaces;

namespace CostAccounting.Application.Services;

public class CostAccountingService : ICostAccountingService
{
    private readonly IShareRepository shareRepository;

    public CostAccountingService(IShareRepository shareRepository)
    {
        this.shareRepository = shareRepository;
    }

    public int GetRemainingSharesNumberAfterSale(int amountToSale)
    {
        var ownedAmount = this.shareRepository.GetSharesAmount();
        
        CheckIfEnoughShares(amountToSale);
        
        var remainingNumber = ownedAmount - amountToSale;
        return remainingNumber;
    }

    public double GetCostBasisOfSoldShares(int amountToSale)
    {
        CheckIfEnoughShares(amountToSale);
        
        var ownedShares = this.shareRepository.GetSharesBalance();
        var soldShares = this.GetSoldShares(ownedShares, amountToSale);

        var costBasis = CalculateCostBasis(soldShares);
        
        return costBasis;
    }

    public double GetCostBasisOfRemainingShares(int amountToSale)
    {
        CheckIfEnoughShares(amountToSale);
        
        var ownedShares = this.shareRepository.GetSharesBalance();
        var remainingShares = this.GetRemainingShares(ownedShares, amountToSale);

        var costBasis = CalculateCostBasis(remainingShares);
        
        return costBasis;
    }

    public double GetProfitOnSale(int amountToSale, double priceForSale)
    {
        CheckIfEnoughShares(amountToSale);
        
        var ownedShares = this.shareRepository.GetSharesBalance();
        var soldShares = this.GetSoldShares(ownedShares, amountToSale);

        var profit = soldShares.Select(x => 
                x.Amount * (priceForSale - x.OriginPrice))
            .Sum();

        return profit;
    }

    private void CheckIfEnoughShares(int amountToSale)
    {
        var ownedAmount = this.shareRepository.GetSharesAmount();
        if (amountToSale > ownedAmount)
        {
            throw new Exception("Not enough shares to perform this operation.");
        }
    }

    private IList<ShareModel> GetSoldShares(IList<PurchasedShareEntity> ownedShares, int amountToSale)
    {
        var amountToSaleCounter = amountToSale;
        List<ShareModel> sold = new();

        foreach (var ownedShare in ownedShares)
        {
            if (amountToSaleCounter != 0)
            {
                var share = new ShareModel
                    { 
                        Id = ownedShare.Id,
                        Amount = ownedShare.Amount < amountToSaleCounter ? ownedShare.Amount : amountToSaleCounter, 
                        OriginPrice = ownedShare.Price 
                    };
                sold.Add(share);
                amountToSaleCounter -= share.Amount;
            }
            else
            {
                break;
            }
        }

        return sold;
    }

    private IList<ShareModel> GetRemainingShares(IList<PurchasedShareEntity> ownedShares, int amountToSale)
    {
        var remaining = new List<ShareModel>();
        var sold = this.GetSoldShares(ownedShares, amountToSale)
            .ToDictionary(x => x.Id);
        
        foreach (var ownedShare in ownedShares)
        {
            var amount = sold.TryGetValue(ownedShare.Id, out var item) 
                ? ownedShare.Amount - item.Amount 
                : ownedShare.Amount;
            
            remaining.Add(new ShareModel
            {
                Id = ownedShare.Id,
                Amount = amount,
                OriginPrice = ownedShare.Price
            });
        }

        return remaining;
    }
    
    private double CalculateCostBasis(IList<ShareModel> shares)
    {
        var amount = shares.Select(x => x.Amount).Sum();
        if (amount == 0)
        {
            return 0;
        }

        var costBasis = shares.Select(x => x.Amount * x.OriginPrice).Sum()
                        / amount;
        
        return costBasis;
    }
    
}