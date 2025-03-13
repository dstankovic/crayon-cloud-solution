using Microsoft.EntityFrameworkCore;

namespace CloudSales.Infrastructure.Persistence;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
}
