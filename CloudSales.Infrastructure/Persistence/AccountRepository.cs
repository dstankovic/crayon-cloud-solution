using CloudSales.Application.Interfaces;
using CloudSales.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace CloudSales.Infrastructure.Persistence;

internal class AccountRepository(DataContext dataContext) : IAccountRepository
{
    public async Task<IEnumerable<AccountViewModel>> GetAccountsModelsAsync(int customerId)
    {
        return await dataContext.Accounts
            .Where(acc => acc.Customer.Id == customerId)
            .Select(acc => AccountViewModel.FromAccount(acc))
            .ToListAsync();
    }

    public async Task<IEnumerable<AccountWithSubscriptionsViewModel>> GetAccountsWithSubscriptionsModelsAsync(int customerId)
    {
        return await dataContext.Accounts
            .Where(acc => acc.Customer.Id == customerId)
            .Select(acc => new AccountWithSubscriptionsViewModel(
                acc.Name,
                acc.Subscriptions.Select(sub => new SubscriptionViewModel(sub.Name, sub.Quantity, sub.State, sub.ValidTo))))
            .ToListAsync();
    }
}
