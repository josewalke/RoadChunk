using UnityEngine;
using System.Collections;

/// <summary>
/// Gestiona la generación de un único chunk con tamaño 13x13x2.
/// </summary>
public class TerrainManager : MonoBehaviour
{
    public GameObject chunkPrefab;      // Prefab que representa un chunk
    public GameObject cubePrefab;       // Prefab que representa un cubo individual
    public Material terrainMaterial;    // Material utilizado para renderizar el terreno
    public Vector3Int chunkSize = new Vector3Int(13, 13, 2); // Dimensiones del chunk (13x13x2)

    [Tooltip("Tiempo en segundos entre cada cubo rojo que aparece en el camino.")]
    public float timeBetweenCubes = 1f; // Tiempo configurable entre cubos

    private GameObject currentChunk;       // Chunk actualmente instanciado
    private int[,,] terrainMatrix;         // Matriz del chunk actual
    private TerrainRenderer terrainRenderer; // Clase que renderiza el terreno
    private IPathGenerator pathGenerator;   // Generador de caminos utilizado

    /// <summary>
    /// Inicializa el sistema al inicio de la escena.
    /// </summary>
    void Start()
    {
        terrainRenderer = new TerrainRenderer(cubePrefab, terrainMaterial);
        pathGenerator = new PathGenerator();

        // Genera el chunk y comienza el camino dinámico
        GenerateChunk();
        StartCoroutine(GeneratePathOverTime());
    }

    /// <summary>
    /// Genera el terreno de tamaño exacto 13x13x2 (cubos verdes inicialmente).
    /// </summary>
    private void GenerateChunk()
    {
        // Inicializa la matriz para el chunk con tamaño 13x13x2
        terrainMatrix = new int[chunkSize.x, chunkSize.y, chunkSize.z];

        // Instancia el chunk en la posición inicial
        if (currentChunk != null)
        {
            Destroy(currentChunk); // Elimina el chunk anterior si existe
        }
        currentChunk = Instantiate(chunkPrefab, Vector3.zero, Quaternion.identity);

        // Renderiza el terreno en dos niveles
        terrainRenderer.RenderAllCubes(currentChunk, terrainMatrix, chunkSize);
    }

    /// <summary>
    /// Genera el camino dinámicamente, un cubo rojo por segundo.
    /// </summary>
    private IEnumerator GeneratePathOverTime()
    {
        Vector3Int startPoint = new Vector3Int(chunkSize.x / 2, chunkSize.y / 2, 0); // Punto inicial del camino
        Vector3Int currentPoint = startPoint;

        // Genera el camino dinámicamente
        while (!pathGenerator.IsOnEdge(currentPoint, chunkSize))
        {
            // Marca la posición actual como parte del camino
            terrainMatrix[currentPoint.x, currentPoint.y, currentPoint.z] = 1;

            // Cambia el cubo existente a rojo
            terrainRenderer.SetCubeAsPath(currentPoint);

            // Obtén el siguiente punto del camino
            currentPoint = pathGenerator.GetNextPoint(currentPoint, chunkSize);

            // Ajusta el tiempo de espera aquí usando la variable configurable
            yield return new WaitForSeconds(timeBetweenCubes);
        }

        // Marca y cambia el último cubo en el borde
        terrainMatrix[currentPoint.x, currentPoint.y, currentPoint.z] = 1;
        terrainRenderer.SetCubeAsPath(currentPoint);

        Debug.Log("Camino completado.");
    }
}
