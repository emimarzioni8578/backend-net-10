using System.ComponentModel.DataAnnotations;

namespace Tareas.Core;

/// <summary>Cuerpo usado para crear o actualizar una tarea.</summary>
public sealed class TareaRequest
{
    [Required, MinLength(1), MaxLength(200)]
    public string Titulo { get; init; } = string.Empty;

    public bool Completada { get; init; }
}
