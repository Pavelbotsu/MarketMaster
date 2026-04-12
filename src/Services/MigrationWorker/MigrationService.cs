using System.ComponentModel;

public class MigrationService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<MigrationService> _logger;

    public MigrationService( IServiceProvider ServiceProvider, ILogger<MigrationService> logger)
    {
        _serviceProvider = ServiceProvider;
        _logger = logger;
    }

   protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Migration Worker started.");

        using var scope = _serviceProvider.CreateScope();
        
        // 1. Get the Central/Master DB Context to find all tenants
        var masterDbContext = scope.ServiceProvider.GetRequiredService<MasterDbContext>();
        var tenants = await masterDbContext.Tenants.ToListAsync(stoppingToken);

        foreach (var tenant in tenants)
        {
            try 
            {
                _logger.LogInformation("Migrating database for Tenant: {TenantId}", tenant.Id);
                
                // 2. Create an instance of the Tenant DB Context with this specific connection string
                var optionsBuilder = new DbContextOptionsBuilder<TenantDbContext>();
                optionsBuilder.UseNpgsql(tenant.ConnectionString); // Use your DB provider

                using var tenantContext = new TenantDbContext(optionsBuilder.Options, null);
                
                // 3. Apply the pending migrations
                await tenantContext.Database.MigrateAsync(stoppingToken);
                
                _logger.LogInformation("Successfully migrated {TenantId}", tenant.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to migrate database for Tenant {TenantId}", tenant.Id);
                // In a real app, you would flag this tenant in the Master DB as "Migration Failed"
            }
        }

        _logger.LogInformation("Migration Worker finished tasks.");
    }
}