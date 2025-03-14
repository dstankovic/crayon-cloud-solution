using CloudSales.Domain.Common;
using CloudSales.Domain.Enums;

namespace CloudSales.Domain.Entities;

public class SoftwareService : BaseEntity
{
    protected SoftwareService() { }

    public SoftwareService(Guid externalId, string name, string description, decimal pricePerLicense, SoftwareServiceState state = SoftwareServiceState.Active)
    {
        Name = name;
        Description = description;
        PricePerLicense = pricePerLicense;
        ExternalId = externalId;
        State = state;
    }

    public Guid ExternalId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal PricePerLicense { get; private set; }
    public SoftwareServiceState State { get; private set; }

    public void Deactivate()
    {
        State = SoftwareServiceState.Inactive;
    }

    public void Update(string name, string description, decimal pricePerLicense)
    {
        Name = name;
        Description = description;
        PricePerLicense = pricePerLicense;
    }
}
