using CloudSales.Application.Models;

namespace CloudSales.Application.Interfaces
{
    public interface IAccountRepository
    {
        Task<IEnumerable<AccountModel>> GetAccountsModelsForCustomerAsync(int customerId);
    }
}
