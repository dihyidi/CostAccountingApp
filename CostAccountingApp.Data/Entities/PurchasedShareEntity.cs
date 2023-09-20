using CostAccounting.Data.Enums;

namespace CostAccounting.Data.Entities;

public class PurchasedShareEntity
{
    public int Id { get; set; }
    
    public int Amount { get; set; }

    public double Price { get; set; }

    public Months Month { get; set; }
}