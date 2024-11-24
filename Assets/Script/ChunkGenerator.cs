using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    private GameObject cubePrefab; // Prefab del cubo
    private Transform chunkContainer; // Contenedor de los bloques del chunk

    public Vector3Int chunkSize = new Vector3Int(13, 13, 2); // Dimensiones del chunk
    public float blockSpacing = 1.0f; // Espaciado entre los bloques

    /// <summary>
    /// Configura el generador de chunks.
    /// </summary>
    public void Initialize(GameObject cubePrefab, Transform chunkContainer)
    {
        this.cubePrefab = cubePrefab;
        this.chunkContainer = chunkContainer;
    }

    void Start()
    {
        GenerateChunk();
    }

    /// <summary>
    /// Genera un chunk de bloques organizados en una cuadrícula.
    /// </summary>
    private void GenerateChunk()
    {
        if (cubePrefab == null || chunkContainer == null)
        {
            Debug.LogError("ChunkGenerator no está inicializado correctamente.");
            return;
        }

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
                    GameObject block = Instantiate(cubePrefab, blockPosition, Quaternion.identity, chunkContainer);

                    // Asignar un nombre consecutivo al bloque
                    block.name = $"Block {blockIndex}";
                    blockIndex++;
                }
            }
        }

        Debug.Log($"Chunk generado con {blockIndex} bloques.");
    }
}
