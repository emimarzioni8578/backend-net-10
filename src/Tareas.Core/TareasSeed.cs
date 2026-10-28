namespace Tareas.Core;

/// <summary>Listado inicial en memoria compartido por ambas APIs.</summary>
public static class TareasSeed
{
    public static List<Tarea> Crear() =>
    [
        new(1, "Comprar leche", false),
        new(2, "Enviar informe semanal", true),
        new(3, "Llamar al dentista", false),
        new(4, "Preparar la presentación", false),
        new(5, "Pagar la factura de la luz", true),
    ];
}
