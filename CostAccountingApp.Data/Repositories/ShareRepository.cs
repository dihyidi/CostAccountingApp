using CostAccounting.Data.Entities;
using CostAccounting.Data.Enums;
using CostAccounting.Data.Repositories.Interfaces;

namespace CostAccounting.Data.Repositories;

public class ShareRepository : IShareRepository
{
    private static readonly List<PurchasedShareEntity> Shares = new()
    {
        new() { Id = 1, Amount = 100, Price = 20, Month = Months.January },
        new() { Id = 2, Amount = 150, Price = 30, Month = Months.February },
        new() { Id = 3, Amount = 120, Price = 10, Month = Months.March },
    };

    public IList<PurchasedShareEntity> GetSharesBalance()
    {
        return Shares;
    }

    public int GetSharesAmount()
    {
        return Shares.Select(x => x.Amount).Sum();
    }
}