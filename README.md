# DBStore

E-commerce de artículos de **Dragon Ball**, construido con .NET 9, PostgreSQL (Supabase) y React.

---

## 🔥 Visión general

- **Funcionalidades**:  
  - Catálogo: listado y detalle de productos  
  - Autenticación: registro/login con JWT  
  - Carrito de compra  
  - Favoritos  
  - Checkout con Mercado Pago  
  - Panel de administración (usuarios, roles, productos, órdenes)  

- **Tecnologías**:  
  - Backend: .NET 9 Web API + EF Core  
  - BD: PostgreSQL en Supabase (RLS habilitado)  
  - Frontend (pendiente): React en Vercel  
  - Autenticación: Supabase Auth (JWT)  
  - Pagos: SDK de Mercado Pago  

---

## 📁 Estructura de la solución

```
DBStore.sln
├── src
│   ├── DBStore.Domain          # Entidades de negocio y contratos (interfaces)
│   │   ├── Entities
│   │   └── Contracts
│   │
│   ├── DBStore.Infrastructure  # EF Core, DbContext, repositorios, conexión a Supabase
│   │   ├── Data                # ApplicationDbContext
│   │   └── Models              # POCOs generados por scaffolding
│   │
│   ├── DBStore.Application     # Casos de uso (services), DTOs, mapeos
│   │
│   └── DBStore.Api             # Controllers, configuración JWT/CORS/Swagger
│
└── tests                       # Proyectos de prueba (xUnit)
```

---

## 🗄 Base de datos (Supabase)

1. Creaste tu proyecto en Supabase con PostgreSQL.  
2. Esquema **public** con tablas:
   - `users`, `roles`, `user_roles`  
   - `products`  
   - `carts`, `cart_items`  
   - `favorites`  
   - `shipping_addresses`  
   - `orders`, `order_items`  
   - `audit_logs`  
3. Habilitaste RLS y configuraste políticas para que:
   - Cada usuario sólo acceda a sus datos (`auth.uid()`)  
   - Admin gestione todo  

---

## 🚀 Scaffolding EF Core

Ejecutado desde la carpeta raíz **DBStore**:

```bash
# 1) Agregar al API
dotnet add src/DBStore.Api package Microsoft.EntityFrameworkCore.Design
dotnet add src/DBStore.Api package Npgsql.EntityFrameworkCore.PostgreSQL

# 2) Definir connection string en Api/appsettings.json
#    (Shared Pooler IPv4, puerto 5432)
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=aws-0-sa-east-1.pooler.supabase.com;Port=5432;Database=postgres;User Id=postgres.ffgemqzyzytivikktofs;Password=Chupame1huevo!;SSL Mode=Require;"
  }
}

# 3) Scaffolding (solo schema public)
dotnet ef dbcontext scaffold Name=DefaultConnection \
  Npgsql.EntityFrameworkCore.PostgreSQL \
  --project src/DBStore.Infrastructure/DBStore.Infrastructure.csproj \
  --startup-project src/DBStore.Api/DBStore.Api.csproj \
  --output-dir src/DBStore.Infrastructure/Models \
  --context-dir src/DBStore.Infrastructure/Data \
  --context ApplicationDbContext \
  --schemas public \
  --use-database-names \
  --force
```

---

## 📦 Capa Domain

**Entities** definidas en `DBStore.Domain/Entities`:

- `User`  
- `Role`  
- `Product`  
- `Cart`, `CartItem`  
- `Favorite`  
- `ShippingAddress`  
- `Order`, `OrderItem`  
- `AuditLog`  

**Contracts** en `DBStore.Domain/Contracts`:

```csharp
public interface IUserRepository { … }
public interface IRoleRepository { … }
public interface IProductRepository { … }
public interface ICartRepository { … }
public interface ICartItemRepository { … }
public interface IFavoriteRepository { … }
public interface IShippingAddressRepository { … }
public interface IOrderRepository { … }
public interface IOrderItemRepository { … }
public interface IAuditLogRepository { … }
```

---

