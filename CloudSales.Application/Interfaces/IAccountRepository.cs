using CloudSales.Application.Models;

namespace CloudSales.Application.Interfaces
{
    public interface IAccountRepository
    {
        Task<IEnumerable<AccountViewModel>> GetAccountsModelsAsync(int customerId);
        Task<IEnumerable<AccountWithSubscriptionsViewModel>> GetAccountsWithSubscriptionsModelsAsync(int customerId);
    }
}
