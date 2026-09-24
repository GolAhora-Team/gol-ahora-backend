# ⚽ Gol Ahora — Backend API

> **API RESTful de alto rendimiento para la gestión integral de complejos deportivos, reservas de canchas, automatización de torneos y cobranzas.**  
> Desarrollado bajo los principios de **Clean Architecture** y **CQRS** con **.NET 8**, **Entity Framework Core** y **SQL Server**.

---

## 📌 Tabla de Contenidos
- [Descripción General](#-descripción-general)
- [Arquitectura de Software](#-arquitectura-de-software)
- [Características y Módulos Principales](#-características-y-módulos-principales)
- [Stack Tecnológico](#-stack-tecnológico)
- [Librerías y Dependencias Clave](#-librerías-y-dependencias-clave)
- [Estructura del Proyecto](#-estructura-del-proyecto)
- [Flujo de CI/CD](#-flujo-de-cicd)
- [Instalación y Ejecución Local](#-instalación-y-ejecución-local)
- [Endpoints y Documentación (Swagger)](#-endpoints-y-documentación-swagger)

---

## 📖 Descripción General

**Gol Ahora** es una plataforma SaaS diseñada para digitalizar la operatoria completa de predios de fútbol y clubes deportivos. Este repositorio contiene el código fuente de la **API Backend**, encargada de resolver la lógica de negocio, la calendarización desatendida de partidos y reservas, el procesamiento de pagos digitales y el control de accesos físicos mediante credenciales codificadas.

---

## 🏛 Arquitectura de Software

El proyecto sigue rigurosamente el patrón **Clean Architecture (Arquitectura Limpia / Onion)** dividido en 4 capas desacopladas, complementado con el patrón **CQRS** (*Command Query Responsibility Segregation*):

```
┌────────────────────────────────────────────────────────┐
│             Presentation (Web API Layer)               │
│        Controllers, Swagger, DI, CORS, Middlewares     │
└──────────────────────────┬─────────────────────────────┘
                           │
┌──────────────────────────▼─────────────────────────────┐
│                 Application Layer                      │
│     Use Cases, Interfaces (Commands/Queries), DTOs,    │
│              Mappers, Domain Exceptions                │
└──────────────────────────┬─────────────────────────────┘
                           │
┌──────────────────────────▼─────────────────────────────┐
│                  Infrastructure Layer                  │
│       EF Core 8, SQL Server, Fluent API, Repositories, │
│        SMTP Email Service, Mercado Pago Client         │
└──────────────────────────┬─────────────────────────────┘
                           │
┌──────────────────────────▼─────────────────────────────┐
│                   Domain Layer                         │
│       Entities, Business Enums, Custom Exceptions      │
└────────────────────────────────────────────────────────┘
```

### Principios aplicados:
- **Inversión de Dependencias (DIP):** Las capas de dominio y aplicación no conocen los detalles de infraestructura ni frameworks externos.
- **CQRS:** Separación explícita entre operaciones de lectura (`I...Query`) y mutaciones/escritura (`I...Command`) para mayor escalabilidad y orden.
- **DTO Pattern:** Contratos independientes para peticiones (`Request`) y respuestas (`Response`), transformados mediante *Mappers* dedicados.
- **Fluent API & Code-First:** Configuración granular de relaciones, tipos y restricciones de bases de datos encapsuladas por entidad (`IEntityTypeConfiguration<T>`).

---

## ⚡ Características y Módulos Principales

### 🏆 1. Motor de Torneos y Fixtures Automatizados
- **Generación de Fixtures para Ligas:** Algoritmo *Round-Robin* (todos contra todos) con alternancia equilibrada de local/visitante y cálculo de jornadas.
- **Cuadros de Eliminación Directa:** Armado automático de llaves para torneos (*Octavos, Cuartos, Semifinal y Final*).
- **Avance Automático de Ganadores:** Al cargar el resultado de un partido (incluyendo penales), el sistema promueve automáticamente al equipo ganador al siguiente cruce del cuadro.
- **Asignación Desatendida de Canchas y Horarios:** Algoritmo que busca franjas horarias libres según el tipo de cancha requerida (Fútbol 5, 7 u 11) y crea las reservas bloqueando los turnos en el calendario.

### 📅 2. Motor de Reservas y Agenda Unificada
- **Calendarización Inteligente:** Proyección dinámica a 30 días que consolida reservas de clientes, partidos oficiales, clases de la academia y entrenamientos.
- **Detección de Conflictos:** Validación de horarios, rangos operativos de canchas y prevención de solapamientos en tiempo real.
- **Políticas de Cancelación Dinámicas:** Cálculo de penalizaciones financieras automáticas en base a las horas de antelación y emisión de cupones de reembolso (100% de descuento) o notas de crédito para socios activos.

### 💳 3. Integración de Pagos con Mercado Pago
- **Checkout Preferences:** Creación de preferencias de pago con soporte dual para entornos de prueba (*Sandbox*) y producción.
- **Procesamiento de Webhooks / IPN:** Listener asíncrono que recibe notificaciones de pago (`approved`), actualiza automáticamente el estado de la reserva a *Confirmada* e imputa el comprobante de pago a la factura correspondiente.

### 🎫 4. Control de Accesos y Generación de Credenciales PDF
- **Pulseras con Código de Barras (Code-128):** Generación vectorial en PDF de brazaletes de acceso para clases y entrenamientos combinando **QuestPDF**, **ZXing.Net** y procesamiento de gráficos con **SkiaSharp**.
- **Reportes Académicos:** Emisión de informes en PDF para el seguimiento de profesores, vigencia de certificados profesionales y asistencias.

### 🔒 5. Gestión de Usuarios, Roles y Documentación
- **Control de Acceso Multirol:** Segmentación entre *Administrador*, *Personal*, *Profesor* y *Cliente*.
- **Recuperación Segura de Contraseñas:** Emisión de tokens de un solo uso con expiración estricta de 1 hora, enviados mediante plantillas de correo HTML transaccionales vía SMTP.
- **Gestión Documental:** Almacenamiento y validación de archivos (aptos físicos médicos y certificaciones docentes en formato PDF con control de tamaño y renombrado con GUIDs).

---

## 🛠 Stack Tecnológico

| Componente | Tecnología | Versión |
| :--- | :--- | :--- |
| **Lenguaje** | C# | 12.0 |
| **Plataforma / Runtime** | .NET | 8.0 |
| **Framework Web** | ASP.NET Core Web API | 8.0 |
| **ORM** | Entity Framework Core | 8.0.20 |
| **Base de Datos** | Microsoft SQL Server | Relacional |
| **Documentación API** | Swagger UI / OpenAPI (Swashbuckle) | 6.6.2 |
| **Generación de PDFs** | QuestPDF | 2026.5.0 |
| **Códigos de Barras** | ZXing.Net / SkiaSharp | 0.16.11 / 3.119.4 |
| **CI / CD** | GitHub Actions & WebDeploy | - |

---

## 📂 Estructura del Proyecto

```text
├── .github/
│   └── workflows/
│       └── deploy.yml             # Pipeline automatizado de CI/CD
├── Aplication/
│   ├── DTOs/                      # Request y Response contracts segmentados por módulo
│   ├── Interfaces/                # Contratos CQRS (Commands, Queries, Services, Mappers)
│   ├── Mappers/                   # Transformación entre Entidades y DTOs
│   └── UseCase/                   # Lógica de negocio y servicios de aplicación
├── Domain/
│   ├── Entities/                  # Entidades de dominio (Cancha, Reserva, Partido, etc.)
│   ├── Enums/                     # Enumeraciones de estado y categorización
│   └── Exceptions/                # Excepciones custom (ExceptionBadRequest, ExceptionNotFound)
├── Infraestructure/
│   ├── Command/                   # Implementaciones de escritura con EF Core
│   ├── Migrations/                # Migraciones históricas de Code-First
│   ├── Persistence/               # AppDbContext y Fluent API (EntityConfigurations)
│   ├── Querys/                    # Implementaciones de consulta optimizada
│   └── Services/                  # Servicios de infraestructura (Email SMTP)
└── SistemaGolAhora/               # Capa de presentación (Host API)
    ├── Controllers/               # Controladores RESTful
    ├── assets/                    # Recursos gráficos (logos para reportes)
    ├── appsettings.json           # Configuración de entornos y cadenas de conexión
    └── Program.cs                 # Inyección de dependencias, CORS, Middleware pipeline
```

---

## 🚀 Flujo de CI/CD

El repositorio cuenta con un pipeline automatizado en **GitHub Actions** ([`.github/workflows/deploy.yml`](.github/workflows/deploy.yml)):

1. **Trigger:** Se activa automáticamente ante cualquier `push` a las ramas `main` o `master`.
2. **Build & Test:** Monta un entorno Windows, instala el SDK de .NET 8 y compila la solución completa en modo `Release`.
3. **Publish:** Empaqueta los artefactos de la aplicación en el directorio de salida.
4. **Deploy:** Despliega de forma continua hacia el servidor remoto mediante **WebDeploy** utilizando credenciales seguras (GitHub Secrets).

---

## 💻 Instalación y Ejecución Local

### Prerrequisitos
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) instalado.
- [Microsoft SQL Server](https://www.microsoft.com/sql-server/) (o SQL Server Express / LocalDB).
- [Git](https://git-scm.com/) instalado.

### Pasos:

1. **Clonar el repositorio:**
   ```bash
   git clone https://github.com/tu-usuario/Gol-Ahora-Backend.git
   cd Gol-Ahora-Backend
   ```

2. **Configurar la cadena de conexión:**  
   En `SistemaGolAhora/appsettings.json`, ajusta la clave `DefaultConnection` para que apunte a tu servidor local de SQL Server:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=GolAhoraDb;Trusted_Connection=True;TrustServerCertificate=True;"
     }
   }
   ```

3. **Restaurar paquetes y compilar:**
   ```bash
   dotnet restore
   dotnet build
   ```

4. **Ejecutar la API:**
   ```bash
   cd SistemaGolAhora
   dotnet run
   ```
   > *Nota:* Al iniciar, la aplicación ejecuta automáticamente `context.Database.Migrate()` en [Program.cs](SistemaGolAhora/Program.cs), por lo que creará las tablas y relaciones en tu base de datos de manera desatendida.

---

## 📑 Endpoints y Documentación (Swagger)

Una vez en ejecución en tu entorno local, accede a la interfaz interactiva de **Swagger UI** ingresando en tu navegador a:

```text
https://localhost:{puerto}/swagger
```

Desde allí podrás explorar, probar e interactuar con todos los endpoints RESTful organizados por módulo:
- `/api/Canchas` — Gestión de canchas y tarifas.
- `/api/Reserva` — Turnos, disponibilidades y cancelaciones.
- `/api/Competicion` y `/api/Partido` — Torneos, ligas y fixtures.
- `/api/MercadoPago` — Preferencias de cobro y recepción de Webhooks.
- `/api/User` y `/api/Clientes` — Autenticación, perfiles y recupero de clave.
- `/api/Factura` y `/api/Pago` — Facturación, cobros y recibos.