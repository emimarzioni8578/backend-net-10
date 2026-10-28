using Microsoft.AspNetCore.Mvc;
using Tareas.Core;

namespace Tareas.ControllersApi.Controllers;

[ApiController]
[Route("api/tareas")]
[Produces("application/json")]
public sealed class TareasController(List<Tarea> tareas) : ControllerBase
{
    /// <summary>Devuelve todas las tareas.</summary>
    [HttpGet]
    [ProducesResponseType<List<Tarea>>(StatusCodes.Status200OK)]
    public ActionResult<List<Tarea>> GetAll()
    {
        var ordenadas = tareas.OrderBy(t => t.Id).ToList();
        return Ok(ordenadas);
    }

    /// <summary>Devuelve una tarea por Id.</summary>
    [HttpGet("{id:int}", Name = nameof(GetById))]
    [ProducesResponseType<Tarea>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Tarea> GetById(int id)
    {
        var tarea = tareas.FirstOrDefault(t => t.Id == id);

        if (tarea is null)
        {
            return NotFound();
        }

        return Ok(tarea);
    }

    /// <summary>Crea una nueva tarea.</summary>
    [HttpPost]
    [ProducesResponseType<Tarea>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Tarea> Create([FromBody] TareaRequest request)
    {
        var nextId = tareas.Count == 0 ? 1 : tareas.Max(t => t.Id) + 1;
        var tarea = new Tarea(nextId, request.Titulo, request.Completada);

        tareas.Add(tarea);

        return CreatedAtRoute(nameof(GetById), new { id = tarea.Id }, tarea);
    }

    /// <summary>Reemplaza una tarea existente.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType<Tarea>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Tarea> Update(int id, [FromBody] TareaRequest request)
    {
        var index = tareas.FindIndex(t => t.Id == id);

        if (index < 0)
        {
            return NotFound();
        }

        var tarea = new Tarea(id, request.Titulo, request.Completada);
        tareas[index] = tarea;

        return Ok(tarea);
    }

    /// <summary>Elimina una tarea.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        var tarea = tareas.FirstOrDefault(t => t.Id == id);

        if (tarea is null)
        {
            return NotFound();
        }

        tareas.Remove(tarea);

        return NoContent();
    }
}
