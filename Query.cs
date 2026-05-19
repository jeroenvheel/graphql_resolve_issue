namespace ResolveWithRepro;

public class Query
{
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Tenant> GetTenants([Service] AppDbContext ctx)
        => ctx.Tenants;
}
