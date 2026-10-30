# Vikingos

Aplicación web para la gestión de vikingos y su ingreso al Valhalla. La interfaz es Blazor Server y consume un Web API de ASP.NET Core. Los datos viven en memoria mientras el proceso de la API está en ejecución.

## Requisitos

- .NET SDK 10
- Certificado de desarrollo HTTPS: `dotnet dev-certs https --trust`

## Ejecutar

Desde la raíz del repositorio, en dos terminales:

```bash
dotnet run --project src/Vikingos.Api --launch-profile https
dotnet run --project src/Vikingos.Web --launch-profile https
```

- API: `https://localhost:7169`
- Blazor: `https://localhost:7170`
- Contrato OpenAPI: `https://localhost:7169/openapi/v1.json`

## Endpoints

| Método | Ruta | Respuesta |
| --- | --- | --- |
| GET | `/api/vikingos` | 200 y la lista |
| GET | `/api/vikingos/{id}` | 200 o 404 |
| POST | `/api/vikingos` | 201 y el vikingo creado |
| PUT | `/api/vikingos/{id}` | 200 o 404 |
| DELETE | `/api/vikingos/{id}` | 204 o 404 |

Un cuerpo inválido responde 400.

## Reglas de Valhalla

Lo que se guarda es el formulario: nombre, batallas ganadas, arma favorita, nivel de honor y causa de muerte gloriosa. Puntos, categoría y el banquete se calculan en cada lectura.

Puntos = `batallas ganadas * 10` más honor: Bajo 0, Medio 40, Alto 80.

Categoría, en este orden:

1. **Guerrero de Thor** si las batallas ganadas son 40 o más.
2. **Guerrero de Odín** si el honor es Alto y las batallas son menos de 40.
3. **Guerrero de Freyja** en cualquier otro caso.

El mensaje "¡Listo para el banquete en Valhalla!" aparece cuando el honor es Alto y los puntos son 150 o más.

Armas: hacha, espada, martillo. Causas de muerte: batalla en campo abierto, duelo uno contra uno, protegiendo su aldea.

## Pruebas

```bash
dotnet test Vikingos.sln
```

El detalle del diseño está en [docs/PLAN-IMPLEMENTACION.md](docs/PLAN-IMPLEMENTACION.md).
