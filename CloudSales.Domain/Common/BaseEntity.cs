﻿namespace CloudSales.Domain.Common;
public class BaseEntity
{
    public int Id { get; private set; }

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
