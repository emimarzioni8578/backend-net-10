namespace Tareas.Core;

/// <summary>Una tarea del listado.</summary>
public sealed record Tarea(int Id, string Titulo, bool Completada);