using CostAccounting.Application.Services;
using CostAccounting.Application.Services.Interfaces;
using CostAccounting.Data.Repositories;
using CostAccounting.Data.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Presentation.Enums;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddScoped<IShareRepository, ShareRepository>();
builder.Services.AddScoped<ICostAccountingService, CostAccountingService>();

using var host = builder.Build();

Run(host.Services);

await host.RunAsync();

static void Run(IServiceProvider hostProvider)
{
    using var serviceScope = hostProvider.CreateScope();
    var provider = serviceScope.ServiceProvider;
    var costAccountingService = provider.GetRequiredService<ICostAccountingService>();
    do
    {
        try
        {
            AccountCost(costAccountingService);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        Console.WriteLine("Do You want to perform another operation? Y / N");
    } while (Console.ReadKey().Key == ConsoleKey.Y);
}

static void AccountCost(ICostAccountingService costAccountingService)
{
    Console.WriteLine("\nPlease enter amount of shares to sale:");
    var amount = Convert.ToInt32(Console.ReadLine());
    if (amount < 0) throw new ArgumentException("Amount cannot be negative.");

    Console.WriteLine("Please enter price of a share to sale:");
    var price = Convert.ToDouble(Console.ReadLine());
    if (price < 0) throw new ArgumentException("Price cannot be negative.");

    Console.WriteLine("Please select an operation:\n" +
                      $"{(int)Operation.GetRemainingSharesNumberAfterSale} - {nameof(Operation.GetRemainingSharesNumberAfterSale)}\n" +
                      $"{(int)Operation.GetCostBasisOfSoldShares} - {nameof(Operation.GetCostBasisOfSoldShares)}\n" +
                      $"{(int)Operation.GetCostBasisOfRemainingShares} - {nameof(Operation.GetCostBasisOfRemainingShares)}\n" +
                      $"{(int)Operation.GetProfitOnSale} - {nameof(Operation.GetProfitOnSale)}\n");

    var operation = (Operation)Convert.ToInt32(Console.ReadLine());

    switch (operation)
    {
        case Operation.GetRemainingSharesNumberAfterSale:
            Console.WriteLine(costAccountingService.GetRemainingSharesNumberAfterSale(amount));
            break;

        case Operation.GetCostBasisOfSoldShares:
            Console.WriteLine(costAccountingService.GetCostBasisOfSoldShares(amount));
            break;

        case Operation.GetCostBasisOfRemainingShares:
            Console.WriteLine(costAccountingService.GetCostBasisOfRemainingShares(amount));
            break;

        case Operation.GetProfitOnSale:
            Console.WriteLine(costAccountingService.GetProfitOnSale(amount, price));
            break;

        default:
            Console.WriteLine("Please select correct operation.");
            break;
    }
}