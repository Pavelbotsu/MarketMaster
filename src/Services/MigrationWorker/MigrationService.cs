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
        
        
        var masterDbContext = scope.ServiceProvider.GetRequiredService<MasterDbContext>();
        var tenants = await masterDbContext.Tenants.ToListAsync(stoppingToken);

        foreach (var tenant in tenants)
        {
            try 
            {
                _logger.LogInformation("Migrating database for Tenant: {TenantId}", tenant.Id);
                
                
                var optionsBuilder = new DbContextOptionsBuilder<TenantDbContext>();
                optionsBuilder.UseNpgsql(tenant.ConnectionString); 

                using var tenantContext = new TenantDbContext(optionsBuilder.Options, null);
                
              
                await tenantContext.Database.MigrateAsync(stoppingToken);
                
                _logger.LogInformation("Successfully migrated {TenantId}", tenant.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to migrate database for Tenant {TenantId}", tenant.Id);
               
            }
        }

        _logger.LogInformation("Migration Worker finished tasks.");
    }
}