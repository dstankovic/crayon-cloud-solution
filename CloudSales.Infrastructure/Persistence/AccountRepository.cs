using CloudSales.Application.Interfaces;
using CloudSales.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace CloudSales.Infrastructure.Persistence;

internal class AccountRepository(DataContext dataContext) : IAccountRepository
{
    public async Task<IEnumerable<AccountModel>> GetAccountsModelsForCustomerAsync(int customerId)
    {
        return await dataContext.Accounts
            .Where(acc => acc.Customer.Id == customerId)
            .Select(acc => new AccountModel(acc.Id, acc.Name, acc.Description))
            .ToListAsync();
    }
}
