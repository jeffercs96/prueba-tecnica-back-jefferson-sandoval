---

# 🎰 Roulette API – .NET 8 + Supabase + EF Core

API backend para un juego de ruleta con apuestas y gestión de billeteras.
Desarrollado como parte de la prueba técnica.

---

## 🚀 Tecnologías

* [.NET 8 Web API](https://dotnet.microsoft.com/)
* [Entity Framework Core](https://learn.microsoft.com/ef/core/)
* [PostgreSQL (Supabase)](https://supabase.com/)
* [Swagger / OpenAPI](https://swagger.io/)

---

## ⚙️ Instalación y ejecución

1. **Clonar el repositorio**

2. **Configurar cadena de conexión** en `appsettings.json`:

   ```json
   "ConnectionStrings": {
     "SupabaseDb": "Host=YOUR_HOST.supabase.co;Port=5432;Database=postgres;Username=YOUR_USER;Password=YOUR_PASSWORD;SSL Mode=Require;Trust Server Certificate=true"
   }
   ```

3. **Aplicar migraciones** (crear tablas en PostgreSQL/Supabase):

   ```bash
   dotnet ef database update
   ```

4. **Ejecutar la API**

   ```bash
   dotnet run
   ```

5. **Abrir Swagger**

   ```
   https://localhost:5001/swagger
   ```

---

## ✨ Autor

Desarrollado por **Jefferson Sandoval**

---
