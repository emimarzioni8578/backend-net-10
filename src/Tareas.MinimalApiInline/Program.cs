using Tareas.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddValidation();
builder.Services.AddOpenApi();

// Listado en memoria compartido por todas las peticiones mientras viva el proceso.
builder.Services.AddSingleton(TareasSeed.Crear());

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Tareas API (Minimal API inline) v1");
        options.DocumentTitle = "Tareas API (Minimal API inline)";
    });
}

app.UseHttpsRedirection();

// ---------------------------------------------------------------------------
// Endpoints: todos definidos aquí mismo, como lambdas, sin clases auxiliares.
// ---------------------------------------------------------------------------

var tareasApi = app.MapGroup("/api/tareas").WithTags("Tareas");

// GET /api/tareas
tareasApi.MapGet("/", (List<Tarea> tareas) =>
{
    var ordenadas = tareas.OrderBy(t => t.Id).ToList();
    return Results.Ok(ordenadas);
})
.WithName("GetTareas")
.Produces<List<Tarea>>(StatusCodes.Status200OK);

// GET /api/tareas/{id}
tareasApi.MapGet("/{id:int}", (int id, List<Tarea> tareas) =>
{
    var tarea = tareas.FirstOrDefault(t => t.Id == id);

    if (tarea is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(tarea);
})
.WithName("GetTareaById")
.Produces<Tarea>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

// POST /api/tareas
tareasApi.MapPost("/", (TareaRequest request, List<Tarea> tareas) =>
{
    var nextId = tareas.Count == 0 ? 1 : tareas.Max(t => t.Id) + 1;
    var tarea = new Tarea(nextId, request.Titulo, request.Completada);

    tareas.Add(tarea);

    return Results.CreatedAtRoute("GetTareaById", new { id = tarea.Id }, tarea);
})
.WithName("CreateTarea")
.Produces<Tarea>(StatusCodes.Status201Created)
.ProducesValidationProblem();

// PUT /api/tareas/{id}
tareasApi.MapPut("/{id:int}", (int id, TareaRequest request, List<Tarea> tareas) =>
{
    var index = tareas.FindIndex(t => t.Id == id);

    if (index < 0)
    {
        return Results.NotFound();
    }

    var tarea = new Tarea(id, request.Titulo, request.Completada);
    tareas[index] = tarea;

    return Results.Ok(tarea);
})
.WithName("UpdateTarea")
.Produces<Tarea>(StatusCodes.Status200OK)
.ProducesValidationProblem()
.Produces(StatusCodes.Status404NotFound);

// DELETE /api/tareas/{id}
tareasApi.MapDelete("/{id:int}", (int id, List<Tarea> tareas) =>
{
    var tarea = tareas.FirstOrDefault(t => t.Id == id);

    if (tarea is null)
    {
        return Results.NotFound();
    }

    tareas.Remove(tarea);

    return Results.NoContent();
})
.WithName("DeleteTarea")
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status404NotFound);

app.Run();
