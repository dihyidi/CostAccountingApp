using System.ComponentModel;

namespace Presentation.Enums;

public enum Operation
{
    [Description("The remaining number of shares after the sale")]
    GetRemainingSharesNumberAfterSale = 1,
    [Description("The cost basis per share of the sold shares")]
    GetCostBasisOfSoldShares,
    [Description("The cost basis per share of the remaining shares after the sale")]
    GetCostBasisOfRemainingShares,
    [Description("The total profit or loss of the sale")]
    GetProfitOnSale
}