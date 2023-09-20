using CostAccounting.Data.Entities;

namespace CostAccounting.Data.Repositories.Interfaces;

public interface IShareRepository
{
    IList<PurchasedShareEntity> GetSharesBalance();
    
    int GetSharesAmount();
}