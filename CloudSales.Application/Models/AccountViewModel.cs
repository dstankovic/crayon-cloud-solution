using CloudSales.Domain.Entities;

namespace CloudSales.Application.Models;

public record AccountViewModel(int Id, string Name, string Description)
{
    public static AccountViewModel FromAccount(Account account)
        => new(account.Id, account.Name, account.Description);
};
