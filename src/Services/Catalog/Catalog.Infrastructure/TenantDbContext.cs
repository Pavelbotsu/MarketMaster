namespace Catalog.Infrastructure.Persistence;


public class TenantDbContext : DbContext

{
    private readonly ITenantProvider _tenantProvider;

    public TenantDbContext(DbContextOptions<TenantDbContext> options, ITenantProvider tenantProvider)
        : base(options)
    {
        _tenantProvider = tenantProvider;
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; } 

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
        var connectionString = _tenantProvider.GetConnectionString();
        optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
