using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using PWA_API.Api.Extensions;
using PWA_API.Api.Middleware;
using PWA_API.Infrastructure.Persistence;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDatabase(builder.Configuration)
    .AddDapper(builder.Configuration)
    .AddRepositories()
    .AddApplicationServices()
    .AddAutoMapperProfiles()
    .AddJwtAuthentication(builder.Configuration);

builder.Services.AddFastEndpoints()
    .SwaggerDocument(o =>
    {
        o.DocumentSettings = s =>
        {
            s.Title = "PWA News API";
            s.Version = "v1";
            s.Description = "API para gestionar un sitio web de noticias de tecnología. Soporta clientes Web y Wearable.";
        };
        o.AutoTagPathSegmentIndex = 0;
    });

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => policy
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());
});

var app = builder.Build();

// Ejecutar migraciones automáticamente al iniciar (de forma no bloqueante)
_ = Task.Run(async () =>
{
    await Task.Delay(2000); // Esperar a que la aplicación inicie
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        logger.LogInformation("Verificando conexión a la base de datos...");
        
        // Intentar conectar con timeout de 10 segundos
        var cts = new CancellationTokenSource(10000);
        if (await db.Database.CanConnectAsync(cts.Token))
        {
            logger.LogInformation("Conexión exitosa. Aplicando migraciones pendientes...");
            var pendingMigrations = await db.Database.GetPendingMigrationsAsync();
            
            if (pendingMigrations.Any())
            {
                logger.LogInformation($"Se encontraron {pendingMigrations.Count()} migraciones pendientes.");
                await db.Database.MigrateAsync();
                logger.LogInformation("Migraciones aplicadas exitosamente.");
            }
            else
            {
                logger.LogInformation("No hay migraciones pendientes.");
            }
            
            logger.LogInformation("Iniciando seed de datos...");
            await DbSeeder.SeedAsync(db);
            logger.LogInformation("Base de datos lista y con datos iniciales.");
        }
        else
        {
            logger.LogWarning("No se pudo conectar a la base de datos en el tiempo esperado.");
        }
    }
    catch (OperationCanceledException)
    {
        logger.LogWarning("Timeout al conectar con la base de datos. La aplicación continuará ejecutándose.");
        logger.LogWarning("Verifica la cadena de conexión y que SQL Server esté ejecutándose.");
        logger.LogWarning("Puedes aplicar migraciones manualmente con:");
        logger.LogWarning("  dotnet ef migrations script -o migration.sql");
        logger.LogWarning("  sqlcmd -S localhost -d NewsDB -i migration.sql -E");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error al inicializar la base de datos: {Message}", ex.Message);
        logger.LogWarning("La aplicación continuará ejecutándose, pero las operaciones de base de datos pueden fallar.");
    }
});

app.UseMiddleware<GlobalExceptionHandler>();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints(c =>
{
    c.Endpoints.RoutePrefix = null;
    c.Errors.ResponseBuilder = (failures, ctx, statusCode) =>
    {
        return new
        {
            status = statusCode,
            errors = failures.Select(f => new { field = f.PropertyName, message = f.ErrorMessage })
        };
    };
});

// FastEndpoints genera el spec en /swagger/v1/swagger.json
app.UseSwaggerGen();

// Scalar apunta al spec de FastEndpoints.Swagger
app.MapScalarApiReference(options =>
{
    options.Title = "PWA News API";
    options.OpenApiRoutePattern = "/swagger/{documentName}/swagger.json";
    options.AddPreferredSecuritySchemes("Bearer");
    options.AddHttpAuthentication("Bearer", auth => auth.Token = "your-jwt-token");
});

// Redirige la raíz a la documentación
app.MapGet("/", () => Results.Redirect("/scalar/v1")).AllowAnonymous();
app.MapGet("/health", () => Results.Ok(new { status = "healthy" })).AllowAnonymous();

app.Run();
