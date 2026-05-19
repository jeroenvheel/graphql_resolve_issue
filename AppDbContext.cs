using Microsoft.EntityFrameworkCore;

namespace ResolveWithRepro;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<Werkgebied> Werkgebieden => Set<Werkgebied>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tenant>().HasKey(t => t.Uuid);
        modelBuilder.Entity<Werkgebied>().HasKey(w => w.Uuid);

        modelBuilder.Entity<Tenant>()
            .HasMany(t => t.Werkgebieden)
            .WithOne(w => w.Tenant)
            .HasForeignKey(w => w.TenantUuid);
    }

    public static void Seed(AppDbContext ctx)
    {
        if (ctx.Tenants.Any()) return;

        var tenantA = new Tenant { Uuid = Guid.NewGuid(), Name = "Tenant A" };
        var tenantB = new Tenant { Uuid = Guid.NewGuid(), Name = "Tenant B" };

        tenantA.Werkgebieden.Add(new Werkgebied { Uuid = Guid.NewGuid(), Name = "Workspace A1", TenantUuid = tenantA.Uuid });
        tenantA.Werkgebieden.Add(new Werkgebied { Uuid = Guid.NewGuid(), Name = "Workspace A2", TenantUuid = tenantA.Uuid });
        tenantB.Werkgebieden.Add(new Werkgebied { Uuid = Guid.NewGuid(), Name = "Workspace B1", TenantUuid = tenantB.Uuid });

        ctx.Tenants.AddRange(tenantA, tenantB);
        ctx.SaveChanges();
    }
}
