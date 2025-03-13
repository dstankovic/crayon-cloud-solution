using CloudSales.Domain.Entities;
using CloudSales.Domain.Enums;

namespace CloudSales.Application.Models
{
    public record AccountWithSubscriptionsViewModel(string Name, IEnumerable<SubscriptionViewModel> Subscriptions)
    {
        public static AccountWithSubscriptionsViewModel FromAccount(Account account) =>
            new(account.Name, account.Subscriptions.Select(SubscriptionViewModel.FromSubscription));
    }

    public record SubscriptionViewModel(string Name, int Quantity, SubscriptionState State, DateTime ValidTo)
    {
        public static SubscriptionViewModel FromSubscription(Subscription subscription) =>
            new(subscription.Name, subscription.Quantity, subscription.State, subscription.ValidTo);
    }
}
