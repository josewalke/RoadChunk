using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    public GameObject cubePrefab; // Prefab del cubo
    public Vector3Int chunkSize = new Vector3Int(13, 13, 2); // Dimensiones del chunk
    public float blockSpacing = 1.0f; // Espaciado entre los bloques

    void Start()
    {
        GenerateChunk();
    }

    /// <summary>
    /// Genera un chunk de bloques organizados en una cuadrícula con nombres consecutivos.
    /// </summary>
    private void GenerateChunk()
    {
        if (cubePrefab == null)
        {
            Debug.LogError("El prefab del cubo no está asignado.");
            return;
        }

        // Contenedor para los bloques del chunk
        GameObject chunkContainer = new GameObject("ChunkContainer");
        chunkContainer.transform.SetParent(transform);

        int blockIndex = 0; // Contador para asignar nombres consecutivos

        // Generar bloques en la cuadrícula
        for (int x = 0; x < chunkSize.x; x++)
        {
            for (int y = 0; y < chunkSize.y; y++)
            {
                for (int z = 0; z < chunkSize.z; z++)
                {
                    // Calcular la posición del bloque
                    Vector3 blockPosition = new Vector3(
                        x * blockSpacing,
                        y * blockSpacing,
                        z * blockSpacing
                    );

                    // Instanciar el bloque
                    GameObject block = Instantiate(cubePrefab, blockPosition, Quaternion.identity, chunkContainer.transform);

                    // Asignar un nombre consecutivo al bloque
                    block.name = $"Block {blockIndex}";
                    blockIndex++; // Incrementar el contador de bloques
                }
            }
        }

        Debug.Log($"Chunk generado con {blockIndex} bloques.");
    }
}
