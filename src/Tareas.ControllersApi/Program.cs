using Tareas.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();

// Listado en memoria compartido por todas las peticiones mientras viva el proceso.
builder.Services.AddSingleton(TareasSeed.Crear());

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    // Documento OpenAPI nativo de .NET en /openapi/v1.json
    app.MapOpenApi();

    // Swagger UI en /swagger, leyendo ese documento
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Tareas API (Controladores) v1");
        options.DocumentTitle = "Tareas API (Controladores)";
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
