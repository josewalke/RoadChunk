using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FirstChunkHandler : MonoBehaviour
{
    public string prefabName = "ChunkPrefab"; // Nombre del prefab a cargar desde Resources
    private GameObject chunkPrefab; // Prefab para generar nuevos chunks
    public Vector3Int chunkSize = new Vector3Int(13, 13, 2); // Tamaño del chunk
    private int[,] level0Matrix; // Matriz para representar el estado de los cubos en el nivel 0
    private List<GameObject> allCubes; // Lista para almacenar todos los cubos en secuencia
    private List<Vector2Int> path; // Lista para almacenar el camino generado

    public float paintInterval = 1f; // Tiempo entre pintar cubos en segundos

    void Start()
    {
        // Cargar el prefab dinámicamente
        LoadChunkPrefab();

        // Inicializa la lista para todos los cubos
        allCubes = new List<GameObject>();

        // Inicializa la matriz
        level0Matrix = new int[chunkSize.x, chunkSize.z];

        // Llena la lista con los cubos generados
        PopulateCubeList();

        // Generar el camino en el primer chunk
        GeneratePathFromCenter();

        // Pintar el camino
        StartCoroutine(PaintPath(paintInterval));
    }

    /// <summary>
    /// Carga el prefab dinámicamente desde la carpeta Resources.
    /// </summary>
    private void LoadChunkPrefab()
    {
        chunkPrefab = Resources.Load<GameObject>(prefabName);

        if (chunkPrefab == null)
        {
            Debug.LogError($"No se encontró el prefab '{prefabName}' en la carpeta Resources.");
        }
        else
        {
            Debug.Log($"Prefab '{prefabName}' cargado correctamente.");
        }
    }

    /// <summary>
    /// Llena la lista con referencias a los cubos hijos.
    /// </summary>
    private void PopulateCubeList()
    {
        foreach (Transform child in transform)
        {
            if (child.name.StartsWith("Block_"))
            {
                allCubes.Add(child.gameObject);
            }
        }

        Debug.Log($"Total de cubos almacenados en la lista: {allCubes.Count}");
    }

    /// <summary>
    /// Genera un camino desde el centro hasta un borde adyacente.
    /// </summary>
    private void GeneratePathFromCenter()
    {
        int startX = chunkSize.x / 2;
        int startZ = chunkSize.z / 2;

        path = new List<Vector2Int>();
        path.Add(new Vector2Int(startX, startZ));

        Vector2Int currentPosition = new Vector2Int(startX, startZ);
        while (!IsAtEdge(currentPosition, chunkSize))
        {
            Vector2Int nextStep = GetRandomDirection(currentPosition, level0Matrix);
            level0Matrix[nextStep.x, nextStep.y] = 1;
            path.Add(nextStep);
            currentPosition = nextStep;
        }

        Debug.Log($"Camino generado con {path.Count} cubos.");
    }

    /// <summary>
    /// Coroutine para pintar el camino cubo por cubo.
    /// </summary>
    /// <param name="interval">Tiempo de espera entre cada pintado.</param>
    private IEnumerator PaintPath(float interval)
    {
        foreach (Vector2Int position in path)
        {
            int cubeIndex = (position.y * chunkSize.x) + position.x;

            GameObject cube = allCubes[cubeIndex];

            if (cube != null)
            {
                Renderer renderer = cube.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = Color.red;
                }
            }

            yield return new WaitForSeconds(interval); // Usar el intervalo proporcionado
        }

        Debug.Log("Camino completado. Generando nuevo chunk...");
        GenerateNewChunk();
    }

    /// <summary>
    /// Genera un nuevo chunk conectado al actual.
    /// </summary>
    private void GenerateNewChunk()
    {
        if (chunkPrefab == null)
        {
            Debug.LogError("chunkPrefab no está asignado. Asegúrate de cargarlo correctamente.");
            return;
        }

        Vector2Int lastPosition = path[path.Count - 1]; // Última posición del camino en el primer chunk
        Vector3 newChunkPosition = transform.position; // Posición base del nuevo chunk

        // Determinar la dirección del nuevo chunk según la salida del camino
        if (lastPosition.x == 0) // Izquierda
            newChunkPosition += new Vector3(-chunkSize.x, 0, 0);
        else if (lastPosition.x == chunkSize.x - 1) // Derecha
            newChunkPosition += new Vector3(chunkSize.x, 0, 0);
        else if (lastPosition.y == 0) // Abajo
            newChunkPosition += new Vector3(0, 0, -chunkSize.z);
        else if (lastPosition.y == chunkSize.z - 1) // Arriba
            newChunkPosition += new Vector3(0, 0, chunkSize.z);

        // Instanciar el nuevo chunk
        GameObject newChunk = Instantiate(chunkPrefab, newChunkPosition, Quaternion.identity);

        Debug.Log("Nuevo chunk generado.");
    }

    /// <summary>
    /// Devuelve true si la posición está en un borde de la matriz.
    /// </summary>
    private bool IsAtEdge(Vector2Int position, Vector3Int size)
    {
        return position.x == 0 || position.x == size.x - 1 || position.y == 0 || position.y == size.z - 1;
    }

    /// <summary>
    /// Devuelve una dirección aleatoria válida desde la posición actual en la matriz dada.
    /// </summary>
    private Vector2Int GetRandomDirection(Vector2Int currentPosition, int[,] matrix)
    {
        List<Vector2Int> directions = new List<Vector2Int>();

        if (currentPosition.x > 0 && matrix[currentPosition.x - 1, currentPosition.y] == 0)
            directions.Add(new Vector2Int(currentPosition.x - 1, currentPosition.y)); // Izquierda

        if (currentPosition.x < chunkSize.x - 1 && matrix[currentPosition.x + 1, currentPosition.y] == 0)
            directions.Add(new Vector2Int(currentPosition.x + 1, currentPosition.y)); // Derecha

        if (currentPosition.y > 0 && matrix[currentPosition.x, currentPosition.y - 1] == 0)
            directions.Add(new Vector2Int(currentPosition.x, currentPosition.y - 1)); // Abajo

        if (currentPosition.y < chunkSize.z - 1 && matrix[currentPosition.x, currentPosition.y + 1] == 0)
            directions.Add(new Vector2Int(currentPosition.x, currentPosition.y + 1)); // Arriba

        return directions[Random.Range(0, directions.Count)];
    }
}
