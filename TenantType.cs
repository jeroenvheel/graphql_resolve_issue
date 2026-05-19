namespace ResolveWithRepro;

public class TenantType : ObjectType<Tenant>
{
    protected override void Configure(IObjectTypeDescriptor<Tenant> descriptor)
    {
        descriptor.Field(t => t.Uuid).IsProjected(true);

        descriptor.Field(t => t.Werkgebieden)
            .UseSorting()
            .UseFiltering()
            .ResolveWith<TenantResolvers>(r => r.GetWorkspaces(default!));
    }
}

public class TenantResolvers
{
    public IQueryable<Werkgebied> GetWorkspaces([Parent] Tenant tenant)
    {
        // Breakpoint here: in v16 this IS hit, but the returned data
        // does not appear in the GraphQL response (werkgebieden is empty/null).
        Console.WriteLine($"[TenantResolvers.GetWorkspaces] called for tenant {tenant.Name} with {tenant.Werkgebieden.Count} workspaces");

        return tenant.Werkgebieden.AsQueryable();
    }
}
