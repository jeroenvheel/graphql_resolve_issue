# HotChocolate v16 — `ResolveWith` returns no data (minimal repro)

This is a minimal reproduction of a regression observed when upgrading HotChocolate from v15 to v16.

When an `ObjectType<T>` configures a field with `ResolveWith<TResolver>(...)`, the resolver method **is invoked** in v16 (confirmed via breakpoint / console log), but the data it returns does **not** appear in the GraphQL response. The field comes back empty/null.

## How to run

```bash
cd graphql_resolve_issue
dotnet run
```

Then open <http://localhost:5080/graphql> (Nitro / Banana Cake Pop) and execute:

```graphql
query {
  tenants {
    uuid
    name
    werkgebieden {
      uuid
      name
    }
  }
}
```

## Expected

Each tenant returns its seeded `werkgebieden`:

```json
{
  "data": {
    "tenants": [
      {
        "uuid": "...",
        "name": "Tenant A",
        "werkgebieden": [
          { "uuid": "...", "name": "Workspace A1" },
          { "uuid": "...", "name": "Workspace A2" }
        ]
      },
      {
        "uuid": "...",
        "name": "Tenant B",
        "werkgebieden": [
          { "uuid": "...", "name": "Workspace B1" }
        ]
      }
    ]
  }
}
```

## Actual (v16)

`TenantResolvers.GetWorkspaces` runs (see console output `[TenantResolvers.GetWorkspaces] called for tenant ...`), but `werkgebieden` in the response is an empty array:

```json
{
  "data": {
    "tenants": [
      {
        "uuid": "e3f1b7ce-2df7-437f-9f7e-602606410e3e",
        "name": "Tenant A",
        "werkgebieden": []
      },
      {
        "uuid": "1f6b0a22-7313-4cf8-9992-4d4de5d2c9f7",
        "name": "Tenant B",
        "werkgebieden": []
      }
    ]
  }
}
```

## Setup

- `Query.GetTenants` is decorated with `[UseProjection] [UseFiltering] [UseSorting]`.
- `TenantType.Werkgebieden` is configured with `.ResolveWith<TenantResolvers>(r => r.GetWorkspaces(default!))`.

See:

- [`Query.cs`](./Query.cs)
- [`TenantType.cs`](./TenantType.cs)
- [`Program.cs`](./Program.cs)

## Versions

- .NET 10
- HotChocolate 16.0.0
- EF Core 10 (InMemory provider for the repro; original codebase uses PostgreSQL)
