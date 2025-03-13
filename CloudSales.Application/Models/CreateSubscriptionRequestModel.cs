
namespace CloudSales.Application.Models;

public record CreateSubscriptionRequestModel(int AccountId, int SoftwareServiceId, int Quantity, DateTime ValidTo);
