namespace CloudSales.Application.Models;

public record OrderLicenseRequestModel(Guid ServiceExtId, int AccountId, int Quantity, DateTime ValidTo);
public record CancelLicenseRequestModel(Guid SubscriptionId);
public record UpdateLicenseRequestModel(Guid SubscriptionId, int Quantity, DateTime ValidTo);
