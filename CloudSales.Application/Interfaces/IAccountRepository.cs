using CloudSales.Application.Models;
using CloudSales.Domain.Entities;

namespace CloudSales.Application.Interfaces
{
    public interface IAccountRepository
    {
        Task<IEnumerable<AccountViewModel>> GetAccountsModelsAsync(int customerId, CancellationToken cancellationToken);
        Task<IEnumerable<AccountWithSubscriptionsViewModel>> GetAccountsWithSubscriptionsModelsAsync(int customerId, CancellationToken cancellationToken);
        Task<Account?> GetAccountAsync(int accountId, CancellationToken cancellationToken);
    }
}
