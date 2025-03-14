using CloudSales.Domain.Common;
using CloudSales.Domain.Enums;

namespace CloudSales.Domain.Entities;

public class Subscription : BaseEntity
{
    protected Subscription() { }

    public Subscription(string name, int quantity, SubscriptionState state, DateTime validTo, Account account, SoftwareService softwareService)
    {
        Name = name;
        Account = account;
        SoftwareService = softwareService;
        AccountId = account.Id;
        SoftwareServiceId = softwareService.Id;

        UpdateQuantity(quantity);
        UpdateState(state);
        ValidTo = AdjustUTC(validTo);
    }

    public int AccountId { get; private set; }
    public virtual Account Account { get; private set; }

    public int SoftwareServiceId { get; private set; }
    public virtual SoftwareService SoftwareService { get; private set; }

    public string Name { get; private set; }
    public int Quantity { get; private set; }
    public SubscriptionState State { get; private set; }
    public DateTime ValidTo { get; private set; }

    public void UpdateState(SubscriptionState state)
    {
        State = state;
    }

    public void UpdateQuantity(int quantity)
    {
        if (quantity < 0)
            throw new ArgumentException("Quantity must not be positive.");

        Quantity = quantity;
    }

    public void UpdateExpiration(DateTime validTo)
    {
        if (ValidTo != default && validTo < ValidTo)
            throw new ArgumentException("Expiration could be just extended.", nameof(validTo));

        ValidTo = AdjustUTC(validTo);
    }

    private static DateTime AdjustUTC(DateTime dateTime)
    {
        return dateTime.Kind == DateTimeKind.Unspecified
         ? DateTime.SpecifyKind(dateTime, DateTimeKind.Utc)
         : dateTime.ToUniversalTime();
    }
}
