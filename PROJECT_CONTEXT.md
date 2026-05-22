# Commerce Backend - Project Context

## Project Goal

Production-grade commerce backend projesi uzerinden C#, ASP.NET Core, PostgreSQL, EF Core, Redis, RabbitMQ, OpenTelemetry, distributed systems ve production backend engineering pratiklerini ogrenmek.

## Current Architecture

- `Commerce.Api`: ASP.NET Core Minimal API, endpoint mapping ve API request DTO'lari.
- `Commerce.Application`: Use case handler'lari ve port interface'leri.
- `Commerce.Domain`: Domain model ve invariant'lar. Su an ana aggregate `Product`.
- `Commerce.Infrastructure`: Repository adapter'lari, clock/id adapter'lari ve EF Core persistence hazirligi.
- `Commerce.Tests`: xUnit unit ve lightweight integration tests.

## Domain

`Product` aggregate su davranislari destekliyor:

- `Create`
- `ChangePrice`
- `Rename`
- `ChangeDescription`
- `Activate`
- `Deactivate`

Onemli invariant'lar:

- Product name bos olamaz.
- Product name en az 3 karakter olmali.
- Currency bos olamaz.
- Price 0 veya negatif olamaz.
- Description icin su an validation yok; empty string kabul ediliyor.

## Application Use Cases

Tamamlanan product use case'leri:

- Create product
- Get product by id
- List products
- Pagination
- Sorting
- Filtering
- Change price
- Deactivate
- Activate
- Change name
- Change description

Application port'lari:

- `IProductRepository`
- `IClock`
- `IIdGenerator`

## Infrastructure

Aktif runtime adapter:

- `IProductRepository -> InMemoryProductRepository`
- `IClock -> SystemClock`
- `IIdGenerator -> GuidIdGenerator`

EF Core hazirligi:

- EF Core, EF Core Design ve Npgsql provider paketleri Infrastructure'a eklendi.
- `CommerceDbContext` eklendi.
- `ProductConfiguration` ile fluent mapping eklendi.
- `CommerceDbContextFactory` ile design-time factory eklendi.
- `InitialCreate` migration olusturuldu, database'e apply edilmedi.
- `EfProductRepository` eklendi ama DI'da aktif degil.

## API Surface

Product endpoint'leri:

- `POST /products`
- `GET /products/{id}`
- `GET /products`
- `PATCH /products/{id}/price`
- `PATCH /products/{id}/deactivate`
- `PATCH /products/{id}/activate`
- `PATCH /products/{id}/name`
- `PATCH /products/{id}/description`

Global exception handler:

- Domain/Application validation kaynakli `ArgumentException` ve `ArgumentOutOfRangeException` -> `400 Bad Request`
- Beklenmeyen exception -> `500 Internal Server Error`

## Testing Status

Son dogrulama:

- `dotnet restore Commerce.slnx` basarili.
- `dotnet build Commerce.slnx` basarili, 0 warning.
- `dotnet test Commerce.slnx` basarili, 89 test gecti.

Test kapsaminda:

- Domain behavior tests
- Application handler tests
- Fake repository tests
- EF InMemory ile `EfProductRepositoryTests`

## Current Baseline

EF Core product persistence groundwork hazir:

- EF Core package setup
- DbContext
- Product EF configuration
- Design-time factory
- Initial migration
- EF product repository
- EF repository integration tests
- `.gitignore` artifact rule

## Next Recommended Task

Gorev 35: EF repository'yi opsiyonel olarak DI'da secilebilir hale getir.

Kural:

- In-memory repository default kalmali.
- Connection string ve explicit flag olmadan EF repository aktif edilmemeli.
- Application katmani EF Core bilmemeye devam etmeli.
