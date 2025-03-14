using CloudSales.Application.Interfaces;
using CloudSales.Application.Models;
using CloudSales.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloudSales.Infrastructure.Persistence;

internal class AccountRepository(DataContext dataContext) : IAccountRepository
{
    public async Task<Account?> GetAccountAsync(int accountId, CancellationToken cancellationToken)
    {
        return await dataContext.Accounts.FirstOrDefaultAsync(acc => acc.Id == accountId, cancellationToken);
    }

    public async Task<IEnumerable<AccountViewModel>> GetAccountsModelsAsync(int customerId, CancellationToken cancellationToken)
    {
        return await dataContext.Accounts
            .Where(acc => acc.Customer.Id == customerId)
            .Select(acc => AccountViewModel.FromAccount(acc))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<AccountWithSubscriptionsViewModel>> GetAccountsWithSubscriptionsModelsAsync(int customerId, CancellationToken cancellationToken)
    {
        return await dataContext.Accounts
            .Where(acc => acc.Customer.Id == customerId)
            .Select(acc => new AccountWithSubscriptionsViewModel(
                acc.Name,
                acc.Subscriptions.Select(sub => new SubscriptionViewModel(sub.Id, sub.Name, sub.Quantity, sub.State, sub.ValidTo))))
            .ToListAsync(cancellationToken);
    }
}
