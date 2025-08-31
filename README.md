# Banking API – README

> **Stack:** .NET 8, Minimal APIs, EF Core, SQL Server/SQLite, xUnit  
> **Arquitectura:** _Vertical Slice_ con Minimal APIs

---

## Requisitos

- **.NET SDK 8.0** o superior
- **EF Core CLI** (opcional si usas terminal):
  ```bash
  dotnet tool update --global dotnet-ef
  ```
- Base de datos de desarrollo:
  - **SQLite** _(multiplataforma)_

Configura tu cadena de conexión en `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=banking.db"
  }
}
```

> **Nota:** El proyecto aplica **migraciones automáticamente al iniciar** (usando `Database.Migrate()` en `Program.cs`). Aun así, abajo te dejo los pasos para lanzarlas manualmente si lo prefieres.

---

## Cómo ejecutar (Visual Studio)

1. Abre la solución (`*.sln`).
2. En el **Solution Explorer**, establece **Banking.Api** como _Startup Project_.
3. Verifica/ajusta la cadena de conexión en `appsettings.Development.json`.
4. Presiona **F5** (Debug) o **Ctrl + F5** (Run).
   - Se crearán/actualizarán las tablas automáticamente (migraciones).
   - La API expone Swagger en `https://localhost:xxxx/swagger` (si está habilitado).

### Ejecutar pruebas en Visual Studio

- Abre **Test → Test Explorer**.
- Clic en **Run All Tests** para ejecutar todas las pruebas del proyecto **Banking.Tests**.

---

## Cómo ejecutar (Terminal / CLI)

Desde la raíz del repo, o ajustando la ruta de proyectos si es necesario.

```bash
# 1) Restaurar y compilar
dotnet restore
dotnet build

# 2) (Opcional) Aplicar migraciones manualmente
#    -p  = proyecto donde está el DbContext (Banking.Api)
#    -s  = proyecto de inicio (Banking.Api)
#    -c  = nombre del DbContext (AppDbContext)

dotnet ef database update -p Banking.Api -s Banking.Api -c AppDbContext

# 3) Levantar la API
dotnet run --project Banking.Api
```

La API quedará disponible en la URL indicada por la consola. Si Swagger está habilitado, podrás probar endpoints en `/swagger`.

### Ejecutar pruebas por CLI

```bash
dotnet test
```

También puedes filtrar pruebas:

```bash
dotnet test --filter TestCategory=Unit
```

---

## Migraciones (manual)

> Repite los flags `-p Banking -s Banking -c AppDbContext` si tu solución tiene varios proyectos.
>
> Si tus nombres de proyecto/contexto difieren, cámbialos acorde.

Crear una nueva migración:

```bash
dotnet ef migrations add InitialCreate -p Banking -s Banking -c AppDbContext
```

Aplicarla a la base de datos:

```bash
dotnet ef database update -p Banking -s Banking -c AppDbContext
```

Revertir la última migración (sin tocar la DB):

```bash
dotnet ef migrations remove -p Banking -s Banking -c AppDbContext
```

**PowerShell (Package Manager Console) en Visual Studio:**

```powershell
Add-Migration InitialCreate -Project Banking -StartupProject Banking -Context AppDbContext
Update-Database -Project Banking -StartupProject Banking -Context AppDbContext
Remove-Migration -Project Banking -StartupProject Banking -Context AppDbContext
```

> **Tip:** Si una migración sale vacía (método `Up()` sin operaciones), revisa que tus **entidades estén mapeadas con `DbSet<T>` en `AppDbContext`** y que los cambios realmente existan.

---

## Estructura (resumen)

- `Banking.Api/`
  - `Features/<Modulo>/<CasoDeUso>/` → endpoint, request/response, validaciones
  - `Domain/` → Entidades de dominio
  - `Persistence/` → `AppDbContext`, configuración EF Core
- `Banking.Tests/` → pruebas unitarias (xUnit); se usa SQLite en memoria para aislar tests

---

## ¿Por qué _Vertical Slice_ con Minimal APIs?

- **Código organizado por casos de uso**, no por capas genéricas: cada “slice” (p. ej., _CreateAccount_, _Deposit_, etc.) agrupa su endpoint, validaciones y lógica.
- **Menos acoplamiento y menor fricción** al cambiar una funcionalidad: tocas solo el slice afectado.
- **Menos ceremonia con Minimal APIs**: arranque rápido, endpoints claros y livianos.
- Escala mejor para **equipos que trabajan por features** y reduce el _“shotgun surgery”_ típico de arquitecturas en capas.

Si no conoces el enfoque, **investígalo**: ayuda a mantener el diseño enfocado en el dominio y los casos de uso reales.

---

## Troubleshooting rápido

- **“No se crean tablas”**: confirma cadena de conexión y que `Database.Migrate()` esté activo. Valida que existan `DbSet<T>` en `AppDbContext`.
- **“PM> Add-Migration compila pero no genera cambios”**: quizá no hay cambios de modelo. Modifica una entidad o configuración y vuelve a ejecutar.
- **Errores de permisos/SSL (SQL Server)**: agrega `TrustServerCertificate=true` a la cadena de conexión en desarrollo.

---
