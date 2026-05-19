namespace ResolveWithRepro;

public class Tenant
{
    public Guid Uuid { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Werkgebied> Werkgebieden { get; set; } = new();
}

public class Werkgebied
{
    public Guid Uuid { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid TenantUuid { get; set; }
    public Tenant? Tenant { get; set; }
}
