using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathPainter : MonoBehaviour
{
    private List<GameObject> level0Cubes; // Cubos del nivel 0
    private Vector3Int chunkSize; // Dimensiones del chunk
    private int[,] pathMatrix; // Matriz que representa el camino en el nivel 0
    private List<Vector2Int> path; // Lista que almacena el camino generado
    private FirstChunkHandler chunkHandler; // Referencia al FirstChunkHandler

    public float paintInterval = 1f; // Tiempo entre pintar cubos en segundos

    /// <summary>
    /// Inicializa la script con los cubos del nivel 0 y el tamaño del chunk.
    /// </summary>
    public void Initialize(List<GameObject> level0Cubes, Vector3Int chunkSize, FirstChunkHandler chunkHandler)
    {
        this.level0Cubes = level0Cubes;
        this.chunkSize = chunkSize;
        this.chunkHandler = chunkHandler;

        // Inicializar la matriz y generar el camino
        InitializePathMatrix();
        GeneratePath();

        // Iniciar la coroutine para pintar el camino
        StartCoroutine(PaintPath());
    }

    /// <summary>
    /// Inicializa la matriz para representar el camino en el nivel 0.
    /// </summary>
    private void InitializePathMatrix()
    {
        pathMatrix = new int[chunkSize.x, chunkSize.z];
        Debug.Log("Matriz de caminos inicializada para el nivel 0.");
    }

    /// <summary>
    /// Genera un camino desde el centro del nivel 0 hasta un borde.
    /// </summary>
    private void GeneratePath()
    {
        path = new List<Vector2Int>();
        int startX = chunkSize.x / 2;
        int startZ = chunkSize.z / 2;

        Vector2Int currentPosition = new Vector2Int(startX, startZ);
        path.Add(currentPosition);
        pathMatrix[startX, startZ] = 1;

        while (!IsAtEdge(currentPosition))
        {
            Vector2Int nextStep = GetRandomDirection(currentPosition);
            path.Add(nextStep);
            pathMatrix[nextStep.x, nextStep.y] = 1;
            currentPosition = nextStep;
        }

        Debug.Log($"Camino generado con {path.Count} bloques.");
    }

    /// <summary>
    /// Devuelve true si la posición está en un borde del chunk.
    /// </summary>
    private bool IsAtEdge(Vector2Int position)
    {
        return position.x == 0 || position.x == chunkSize.x - 1 || position.y == 0 || position.y == chunkSize.z - 1;
    }

    /// <summary>
    /// Devuelve una dirección aleatoria válida para continuar el camino.
    /// </summary>
    private Vector2Int GetRandomDirection(Vector2Int currentPosition)
    {
        List<Vector2Int> directions = new List<Vector2Int>();

        if (currentPosition.x > 0 && pathMatrix[currentPosition.x - 1, currentPosition.y] == 0)
            directions.Add(new Vector2Int(currentPosition.x - 1, currentPosition.y)); // Izquierda

        if (currentPosition.x < chunkSize.x - 1 && pathMatrix[currentPosition.x + 1, currentPosition.y] == 0)
            directions.Add(new Vector2Int(currentPosition.x + 1, currentPosition.y)); // Derecha

        if (currentPosition.y > 0 && pathMatrix[currentPosition.x, currentPosition.y - 1] == 0)
            directions.Add(new Vector2Int(currentPosition.x, currentPosition.y - 1)); // Abajo

        if (currentPosition.y < chunkSize.z - 1 && pathMatrix[currentPosition.x, currentPosition.y + 1] == 0)
            directions.Add(new Vector2Int(currentPosition.x, currentPosition.y + 1)); // Arriba

        return directions[Random.Range(0, directions.Count)];
    }

    /// <summary>
    /// Coroutine para pintar el camino cubo por cubo.
    /// </summary>
    private IEnumerator PaintPath()
    {
        Debug.Log("Iniciando la coroutine PaintPath.");

        if (path == null || path.Count == 0)
        {
            Debug.LogError("No se generó ningún camino.");
            yield break;
        }

        foreach (Vector2Int position in path)
        {
            // Calcular el índice del bloque en la lista según su posición en la matriz
            int blockIndex = (position.y * chunkSize.x) + position.x;
            if (blockIndex >= 0 && blockIndex < level0Cubes.Count)
            {
                GameObject block = level0Cubes[blockIndex];
                Renderer renderer = block.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = Color.red;
                    Debug.Log($"Pintando bloque en posición: {position.x}, 0, {position.y}");
                }
                else
                {
                    Debug.LogError($"El bloque {block.name} no tiene Renderer.");
                }
            }
            else
            {
                Debug.LogError($"Índice de bloque fuera de rango: {blockIndex}");
            }

            yield return new WaitForSeconds(paintInterval);
        }

        Debug.Log("Camino pintado completamente en el nivel 0.");

        // Crear un nuevo chunk al terminar el camino
        Vector3 exitDirection = GetExitDirection();
        chunkHandler.CreateNewChunk(exitDirection);
    }

    /// <summary>
    /// Determina la dirección de salida del camino desde el chunk.
    /// </summary>
    private Vector3 GetExitDirection()
    {
        Vector2Int lastPosition = path[path.Count - 1];

        if (lastPosition.x == 0) return Vector3.left;
        if (lastPosition.x == chunkSize.x - 1) return Vector3.right;
        if (lastPosition.y == 0) return Vector3.back;
        if (lastPosition.y == chunkSize.z - 1) return Vector3.forward;

        return Vector3.zero; // Caso de error
    }
}
