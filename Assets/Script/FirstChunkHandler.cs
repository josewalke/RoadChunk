using UnityEngine;
using System.Collections.Generic;

public class FirstChunkHandler : MonoBehaviour
{
    public GameObject cubePrefab; // Prefab del cubo
    private GameObject chunkPrefab; // Prefab del chunk cargado por código
    public Vector3Int chunkSize = new Vector3Int(13, 13, 2); // Dimensiones del chunk
    public float blockSpacing = 1.0f; // Espaciado entre los bloques

    private List<GameObject> level0Cubes; // Lista que almacena todos los cubos del nivel 0

    void Start()
    {
        // Cargar el prefab del chunk
        LoadChunkPrefab();

        // Generar el chunk inicial
        GenerateChunk();

        // Añadir la script PathPainter y pasarle los cubos del nivel 0
        AddPathPainter();
    }

    /// <summary>
    /// Carga el prefab del chunk desde la carpeta Resources.
    /// </summary>
    private void LoadChunkPrefab()
    {
        chunkPrefab = Resources.Load<GameObject>("ChunkPrefab");
        if (chunkPrefab == null)
        {
            Debug.LogError("No se encontró el prefab del chunk en ChunkPrefab.");
        }
        else
        {
            Debug.Log("Prefab del chunk cargado correctamente.");
        }
    }

    /// <summary>
    /// Genera un chunk de bloques organizados en una cuadrícula 3D y guarda los cubos del nivel 0.
    /// </summary>
    private void GenerateChunk()
    {
        level0Cubes = new List<GameObject>();
        int blockIndex = 0; // Contador para asignar nombres consecutivos

        for (int x = 0; x < chunkSize.x; x++) // Recorrer ancho
        {
            for (int y = 0; y < chunkSize.y; y++) // Recorrer niveles (altura)
            {
                for (int z = 0; z < chunkSize.z; z++) // Recorrer profundidad
                {
                    // Calcular la posición del bloque
                    Vector3 blockPosition = new Vector3(
                        x * blockSpacing,
                        y * blockSpacing,
                        z * blockSpacing
                    );

                    // Instanciar el bloque
                    GameObject block = Instantiate(cubePrefab, blockPosition, Quaternion.identity, transform);

                    // Asignar un nombre consecutivo al bloque
                    block.name = $"Block {blockIndex}";
                    blockIndex++;

                    // Si el bloque está en el nivel 0, guardarlo
                    if (y == 0)
                    {
                        level0Cubes.Add(block);
                    }
                }
            }
        }

        Debug.Log($"Chunk generado con {blockIndex} bloques. Nivel 0 tiene {level0Cubes.Count} bloques.");
    }

    /// <summary>
    /// Añade la script PathPainter al contenedor del chunk y le pasa los cubos del nivel 0.
    /// </summary>
    private void AddPathPainter()
    {
        PathPainter pathPainter = gameObject.AddComponent<PathPainter>();
        pathPainter.Initialize(level0Cubes, chunkSize, this);

        Debug.Log("Script PathPainter añadido y configurado.");
    }

    /// <summary>
    /// Genera un nuevo chunk en la dirección adyacente al último bloque del camino.
    /// </summary>
    public void CreateNewChunk(Vector3 exitDirection)
    {
        if (chunkPrefab == null)
        {
            Debug.LogError("Prefab del chunk no cargado. Asegúrate de que esté en ChunkPrefab.");
            return;
        }

        // Calcular la posición del nuevo chunk
        Vector3 newChunkPosition = transform.position + exitDirection * (chunkSize.x * blockSpacing);

        // Instanciar el nuevo chunk
        GameObject newChunk = Instantiate(chunkPrefab, newChunkPosition, Quaternion.identity);

        // Añadir el componente ChunkGenerator al nuevo chunk
        ChunkGenerator chunkGenerator = newChunk.AddComponent<ChunkGenerator>();
        chunkGenerator.Initialize(cubePrefab, newChunk.transform);
        chunkGenerator.chunkSize = chunkSize;

        Debug.Log($"Nuevo chunk creado en la posición: {newChunkPosition} con el componente ChunkGenerator añadido.");
    }
}
