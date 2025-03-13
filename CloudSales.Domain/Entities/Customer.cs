using CloudSales.Domain.Common;
namespace CloudSales.Domain.Entities;

public class Customer : BaseEntity
{
    private Customer() { }

    public Customer(string name, string email, string phoneNumber)
    {
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
    }

    public string Name { get; private set; }
    public string Email { get; private set; }
    public string PhoneNumber { get; private set; }

    private List<User> _users = [];
    public virtual IReadOnlyCollection<User> Users => _users.AsReadOnly();

    private List<Account> _accounts = [];
    public virtual IReadOnlyCollection<Account> Accounts => _accounts.AsReadOnly();
}
