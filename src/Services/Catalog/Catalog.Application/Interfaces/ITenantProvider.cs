namespace Catalog.Application.Interfaces;
public interface ITenantProvider
{
    Guid GetTenantId();
    string GetConnectionString();
}