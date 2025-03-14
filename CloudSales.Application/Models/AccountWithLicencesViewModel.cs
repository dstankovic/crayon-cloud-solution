using CloudSales.Domain.Enums;

namespace CloudSales.Application.Models
{
    public record AccountWithSubscriptionsViewModel(string Name, IEnumerable<SubscriptionViewModel> Subscriptions);

    public record SubscriptionViewModel(int Id, string Name, int Quantity, SubscriptionState State, DateTime ValidTo);
}
