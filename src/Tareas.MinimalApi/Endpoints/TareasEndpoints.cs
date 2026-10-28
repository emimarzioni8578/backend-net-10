using Microsoft.AspNetCore.Http.HttpResults;
using Tareas.Core;

namespace Tareas.MinimalApi.Endpoints;

public static class TareasEndpoints
{
    public static IEndpointRouteBuilder MapTareasEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/tareas").WithTags("Tareas");

        group.MapGet("/", GetAll).WithName("GetTareas");
        group.MapGet("/{id:int}", GetById).WithName("GetTareaById");
        group.MapPost("/", Create).WithName("CreateTarea");
        group.MapPut("/{id:int}", Update).WithName("UpdateTarea");
        group.MapDelete("/{id:int}", Delete).WithName("DeleteTarea");

        return app;
    }

    private static Ok<List<Tarea>> GetAll(List<Tarea> tareas)
    {
        var ordenadas = tareas.OrderBy(t => t.Id).ToList();
        return TypedResults.Ok(ordenadas);
    }

    private static Results<Ok<Tarea>, NotFound> GetById(int id, List<Tarea> tareas)
    {
        var tarea = tareas.FirstOrDefault(t => t.Id == id);

        if (tarea is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(tarea);
    }

    private static CreatedAtRoute<Tarea> Create(TareaRequest request, List<Tarea> tareas)
    {
        var nextId = tareas.Count == 0 ? 1 : tareas.Max(t => t.Id) + 1;
        var tarea = new Tarea(nextId, request.Titulo, request.Completada);

        tareas.Add(tarea);

        return TypedResults.CreatedAtRoute(tarea, "GetTareaById", new { id = tarea.Id });
    }

    private static Results<Ok<Tarea>, NotFound> Update(int id, TareaRequest request, List<Tarea> tareas)
    {
        var index = tareas.FindIndex(t => t.Id == id);

        if (index < 0)
        {
            return TypedResults.NotFound();
        }

        var tarea = new Tarea(id, request.Titulo, request.Completada);
        tareas[index] = tarea;

        return TypedResults.Ok(tarea);
    }

    private static Results<NoContent, NotFound> Delete(int id, List<Tarea> tareas)
    {
        var tarea = tareas.FirstOrDefault(t => t.Id == id);

        if (tarea is null)
        {
            return TypedResults.NotFound();
        }

        tareas.Remove(tarea);

        return TypedResults.NoContent();
    }
}
