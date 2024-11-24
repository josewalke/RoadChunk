using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    public GameObject cubePrefab; // Prefab del cubo
    public Vector3Int chunkSize = new Vector3Int(13, 13, 2); // Tamaño del chunk

    void Start()
    {
        if (cubePrefab == null)
        {
            Debug.LogError("Prefab de cubo no asignado.");
            return;
        }

        // Crear el contenedor para el primer chunk
        GameObject chunkContainer = new GameObject("Chunk");
        chunkContainer.transform.SetParent(transform);

        // Crear el generador del chunk
        ChunkGenerator chunkGenerator = new ChunkGenerator(cubePrefab, chunkContainer.transform);

        // Generar el primer chunk
        chunkGenerator.GenerateChunk(chunkSize);

        // Asignar la script FirstChunkHandler al contenedor del chunk
        AddFirstChunkHandler(chunkContainer);
    }

    /// <summary>
    /// Asigna la script FirstChunkHandler al contenedor del primer chunk.
    /// </summary>
    /// <param name="chunkContainer">El GameObject que contiene el primer chunk.</param>
    private void AddFirstChunkHandler(GameObject chunkContainer)
    {
        // Añadir la script FirstChunkHandler
        FirstChunkHandler firstChunkHandler = chunkContainer.AddComponent<FirstChunkHandler>();

        // Configurar el tamaño del chunk en la script
        firstChunkHandler.chunkSize = chunkSize;

        Debug.Log("Script FirstChunkHandler asignado al primer chunk.");
    }
}
