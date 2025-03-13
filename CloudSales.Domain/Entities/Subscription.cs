using CloudSales.Domain.Common;
using CloudSales.Domain.Enums;

namespace CloudSales.Domain.Entities;

public class Subscription : BaseAggregatedEntity
{
    public Subscription() { }

    public Subscription(string name, int quantity, SubscriptionState state, DateTime validTo, Account account, SoftwareService softwareService)
    {
        Name = name;
        Quantity = quantity;
        State = state;
        ValidTo = validTo;
        Account = account;
        SoftwareService = softwareService;

        AccountId = account.Id;
        SoftwareServiceId = softwareService.Id;
    }

    public int AccountId { get; private set; }
    public virtual Account Account { get; private set; }

    public int SoftwareServiceId { get; private set; }
    public virtual SoftwareService SoftwareService { get; private set; }

    public string Name { get; private set; }
    public int Quantity { get; private set; }
    public SubscriptionState State { get; private set; }
    public DateTime ValidTo { get; private set; }
}
