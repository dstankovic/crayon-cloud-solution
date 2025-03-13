namespace CloudSales.Application.Models;

public record OrderLicenseRequestModel(Guid ServiceExtId, int AccountId, int Quantity, DateTime ValidTo);
