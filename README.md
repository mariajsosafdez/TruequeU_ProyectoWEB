# 🔄 TruequeU - Backend
> Equipo 9: Maria José Sosa, Julián Marín

Este repositorio contiene la lógica de servidor, persistencia de datos y API REST para el proyecto TruequeU, una plataforma de intercambios que fomenta la economía circular y el apoyo entre los estudiantes de la Universidad EIA.

## Estructura y gestión
* Framework: .NET 10.0 (ASP.NET Core)
* Arquitectura por Capas (Controladores → Interfaces → Servicios → DB)
* Persistencia: SQL Server + Entity Framework Core
* Auth: ASP.NET Core Identity + JWT Bearer

## Instalación de Dependencias
Ejecutar en la *Package Manager Console*:
```powershell
Install-Package Scalar.AspNetCore -Version 2.14.3
Install-Package Microsoft.AspNetCore.OpenApi -Version 10.0.6
Install-Package Microsoft.AspNetCore.Authentication.JwtBearer -Version 10.0.7
Install-Package Microsoft.AspNetCore.Identity.EntityFrameworkCore -Version 10.0.7
Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 10.0.7
Install-Package Microsoft.EntityFrameworkCore.Design -Version 10.0.7
Install-Package Microsoft.EntityFrameworkCore.Tools -Version 10.0.7