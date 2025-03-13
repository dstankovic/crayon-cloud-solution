﻿namespace CloudSales.Domain.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(int entityId, string entityName) : base($"Entity {entityName} with an Id: {entityId} could not be found.") { }

        public EntityNotFoundException(string message) : base(message) { }
    }
}
