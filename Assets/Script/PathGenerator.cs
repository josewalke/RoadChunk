using UnityEngine;

/// <summary>
/// Generador de caminos dentro de un chunk.
/// </summary>
public class PathGenerator : IPathGenerator
{
    /// <summary>
    /// Genera un camino dentro de la matriz desde un punto inicial.
    /// </summary>
    /// <param name="matrix">Matriz tridimensional del chunk.</param>
    /// <param name="chunkSize">Tamaño del chunk.</param>
    /// <param name="startPoint">Punto inicial del camino dentro del chunk.</param>
    public void GeneratePath(int[,,] matrix, Vector3Int chunkSize, Vector3Int startPoint)
    {
        Vector3Int currentPoint = startPoint;

        // Genera el camino completo en un solo paso
        while (!IsOnEdge(currentPoint, chunkSize))
        {
            // Marca la posición actual como parte del camino
            matrix[currentPoint.x, currentPoint.y, currentPoint.z] = 1;

            // Obtiene el siguiente punto en el camino
            currentPoint = GetNextPoint(currentPoint, chunkSize);
        }

        // Marca la última posición en el borde
        matrix[currentPoint.x, currentPoint.y, currentPoint.z] = 1;
    }

    /// <summary>
    /// Verifica si un punto está en el borde del chunk.
    /// </summary>
    public bool IsOnEdge(Vector3Int point, Vector3Int chunkSize)
    {
        return point.x == 0 || point.x == chunkSize.x - 1 || point.y == 0 || point.y == chunkSize.y - 1;
    }

    /// <summary>
    /// Obtiene el siguiente punto válido del camino.
    /// </summary>
    public Vector3Int GetNextPoint(Vector3Int currentPoint, Vector3Int chunkSize)
    {
        Vector3Int[] directions = {
            new Vector3Int(1, 0, 0),  // Derecha
            new Vector3Int(-1, 0, 0), // Izquierda
            new Vector3Int(0, 1, 0),  // Arriba
            new Vector3Int(0, -1, 0)  // Abajo
        };

        // Selecciona una dirección aleatoria
        Vector3Int direction = directions[Random.Range(0, directions.Length)];
        Vector3Int nextPoint = currentPoint + direction;

        // Verifica si el punto es válido
        if (IsValid(nextPoint, chunkSize))
        {
            return nextPoint;
        }

        // Si no es válido, intenta nuevamente
        return GetNextPoint(currentPoint, chunkSize);
    }

    /// <summary>
    /// Verifica si un punto es válido dentro de los límites del chunk.
    /// </summary>
    private bool IsValid(Vector3Int point, Vector3Int chunkSize)
    {
        return point.x >= 0 && point.x < chunkSize.x &&
               point.y >= 0 && point.y < chunkSize.y &&
               point.z == 0; // Restricción para estar en la planta baja
    }
}
