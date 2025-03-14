namespace CloudSales.Application.Models;

public record OrderLicenseRequestModel(Guid ServiceExtId, int AccountId, int Quantity, DateTime ValidTo);
public record CancelLicenseRequestModel(Guid ServiceExtId, int AccountId);
public record UpdateLicenseRequestModel(Guid ServiceExtId, int AccountId, int Quantity, DateTime ValidTo);
