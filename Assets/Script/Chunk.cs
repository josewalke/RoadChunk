using UnityEngine;

/// <summary>
/// Representa un chunk individual del terreno.
/// Maneja la inicialización y almacenamiento de la matriz del chunk.
/// </summary>
public class Chunk : MonoBehaviour, IChunk
{
    private Vector3Int size; // Dimensiones del chunk
    private int[,,] matrix;  // Matriz tridimensional que define el contenido del chunk

    /// <summary>
    /// Inicializa el chunk con sus dimensiones y matriz de contenido.
    /// </summary>
    /// <param name="size">Tamaño del chunk (ancho, alto, profundidad).</param>
    /// <param name="matrix">Matriz que define el terreno.</param>
    public void Initialize(Vector3Int size, int[,,] matrix)
    {
        this.size = size;
        this.matrix = matrix;
    }

    /// <summary>
    /// Obtiene el tamaño del chunk.
    /// </summary>
    /// <returns>Un Vector3Int representando las dimensiones del chunk.</returns>
    public Vector3Int GetSize()
    {
        return size;
    }

    /// <summary>
    /// Obtiene la matriz del chunk.
    /// </summary>
    /// <returns>La matriz tridimensional del chunk.</returns>
    public int[,,] GetMatrix()
    {
        return matrix;
    }
}
