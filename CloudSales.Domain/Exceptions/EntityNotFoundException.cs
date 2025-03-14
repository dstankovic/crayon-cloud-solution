namespace CloudSales.Domain.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public int EntityId { get; }
        public string EntityName { get; }

        public EntityNotFoundException(int entityId, string entityName) : base($"Entity {entityName} with an Id: {entityId} could not be found.")
        {
            EntityId = entityId;
            EntityName = entityName;
        }

        public EntityNotFoundException(string message) : base(message) { }
    }
}
