using Microsoft.EntityFrameworkCore;
using ResolveWithRepro;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseInMemoryDatabase("ResolveWithRepro"));

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddType<TenantType>()
    .AddProjections()
    .AddFiltering()
    .AddSorting();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    AppDbContext.Seed(ctx);
}

app.MapGraphQL();

app.Run();
