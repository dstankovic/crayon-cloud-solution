using CloudSales.Domain.Common;

namespace CloudSales.Domain.Entities;

public class SoftwareService : BaseEntity
{
    protected SoftwareService() { }

    public SoftwareService(Guid externalId, string name, string description, decimal pricePerLicense)
    {
        Name = name;
        Description = description;
        PricePerLicense = pricePerLicense;
        ExternalId = externalId;
    }

    public Guid ExternalId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal PricePerLicense { get; private set; }
}
