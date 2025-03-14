using CloudSales.Domain.Common;

namespace CloudSales.Domain.Entities;

public class Account : BaseEntity
{
    protected Account() { }

    public Account(string name, string description, Customer customer)
    {
        Name = name;
        Description = description;
        Customer = customer;
    }

    public string Name { get; private set; }
    public string Description { get; private set; }

    public int CustomerId { get; private set; }
    public virtual Customer Customer { get; private set; }

    private readonly List<Subscription> _subscriptions = [];
    public virtual IReadOnlyCollection<Subscription> Subscriptions => _subscriptions.AsReadOnly();
}
