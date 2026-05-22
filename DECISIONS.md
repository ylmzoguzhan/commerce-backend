# Commerce Backend - Architecture Decisions

## ADR-001: Application EF Core Bilmez

Application handler'lari `DbContext` kullanmaz. Persistence ihtiyaci `IProductRepository` port'u uzerinden ifade edilir.

Sonuc:

- Application katmani EF Core veya PostgreSQL'e bagimli olmaz.
- Unit testlerde fake repository kullanilabilir.
- Infrastructure adapter degisiklikleri use case handler'larini etkilemez.

## ADR-002: Domain EF Attribute Icermez

Domain entity'lerine `[Table]`, `[Key]`, `[Required]` gibi EF attribute'lari eklenmez. Mapping Infrastructure katmaninda Fluent API ile yapilir.

Sonuc:

- Domain modeli persistence teknolojisinden bagimsiz kalir.
- EF mapping review'lari `ProductConfiguration` gibi siniflarda yapilir.

## ADR-003: InMemory Repository Default Adapter Olarak Kalir

EF repository hazirlansa bile runtime DI su an `IProductRepository -> InMemoryProductRepository` kullanir.

Sonuc:

- EF gecisi kademeli yapilir.
- DbContext, migration ve connection string hazir olmadan API davranisi bozulmaz.

## ADR-004: Clock ve Id Generation Port Olarak Soyutlandi

Zaman ve id uretimi Application port'lari ile temsil edilir:

- `IClock`
- `IIdGenerator`

Infrastructure adapter'lari:

- `SystemClock`
- `GuidIdGenerator`

Sonuc:

- Create product use case'i deterministik test edilebilir.
- Domain `DateTimeOffset.UtcNow` ve `Guid.NewGuid()` gibi dis dunya cagri detaylarina mahkum kalmaz.

## ADR-005: Product Creation Explicit Id ve CreatedAt Destekler

`Product.Create` id ve createdAt alabilen overload destekler. Kolay factory overload'u da korunur.

Sonuc:

- Application handler id ve createdAt'i port'lardan alarak Product'a verir.
- Test, import, migration ve replay senaryolari icin deterministik creation mumkun olur.

## ADR-006: EF Design-Time Factory Runtime Config Degildir

`CommerceDbContextFactory` sadece EF migration tooling icindir. Runtime connection string yonetimi API/configuration tarafinda ele alinacaktir.

Sonuc:

- Local connection string factory icinde bulunabilir.
- `Program.cs` icine local connection string hardcode edilmez.

## ADR-007: PostgreSQL Schema Snake Case Kullanir

Database table/column isimleri snake_case tutulur:

- table: `products`
- columns: `id`, `name`, `description`, `price`, `currency`, `is_active`, `created_at`, `updated_at`

Sonuc:

- C# property'leri PascalCase kalir.
- PostgreSQL tarafinda quoted PascalCase kolonlarla calisma riski azalir.

## ADR-008: EF Repository Query Pipeline Database Tarafinda Kalir

EF repository filtreleme, siralama ve pagination icin `IQueryable` pipeline kullanir. Erken `AsEnumerable()` veya `ToList()` kullanilmaz.

Sonuc:

- Filtering, sorting ve pagination SQL'e cevrilebilir.
- Buyuk veri setleri memory'ye gereksiz yuklenmez.

## ADR-009: EF InMemory Testleri PostgreSQL Garantisi Degildir

`EfProductRepositoryTests` EF InMemory provider ile davranis regression guard'i saglar; PostgreSQL-specific davranis garantisi vermez.

Sonuc:

- Repository logic hizli test edilir.
- Ileride PostgreSQL/Testcontainers tabanli integration test stratejisi ayrica ele alinacaktir.
