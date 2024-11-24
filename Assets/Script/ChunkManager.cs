using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    public GameObject cubePrefab; // Prefab del cubo
    public Vector3Int chunkSize = new Vector3Int(13, 13, 2); // Tamaño del chunk

    void Start()
    {
        // Crear el contenedor para el chunk
        GameObject chunkContainer = new GameObject("ChunkContainer");
        chunkContainer.transform.SetParent(transform);

        // Añadir y configurar FirstChunkHandler primero
        FirstChunkHandler firstChunkHandler = AddFirstChunkHandler(chunkContainer);

        // Configurar el tamaño del chunk en FirstChunkHandler
        firstChunkHandler.chunkSize = chunkSize;

        Debug.Log("Script FirstChunkHandler asignado y configurado al primer chunk.");
    }

    /// <summary>
    /// Asigna la script FirstChunkHandler al contenedor del primer chunk.
    /// </summary>
    /// <param name="chunkContainer">El GameObject que contiene el primer chunk.</param>
    /// <returns>La instancia de FirstChunkHandler añadida.</returns>
    private FirstChunkHandler AddFirstChunkHandler(GameObject chunkContainer)
    {
        // Añadir la script FirstChunkHandler
        FirstChunkHandler firstChunkHandler = chunkContainer.AddComponent<FirstChunkHandler>();

        // Configurar el prefab del cubo
        firstChunkHandler.cubePrefab = cubePrefab;

        Debug.Log("FirstChunkHandler configurado.");
        return firstChunkHandler;
    }
}
