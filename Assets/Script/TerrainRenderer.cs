using UnityEngine;

/// <summary>
/// Renderiza cubos dentro de un chunk.
/// </summary>
public class TerrainRenderer
{
    private GameObject cubePrefab;
    private Material terrainMaterial;
    private GameObject[,,] cubes; // Matriz para almacenar referencias a los cubos

    public TerrainRenderer(GameObject cubePrefab, Material terrainMaterial)
    {
        this.cubePrefab = cubePrefab;
        this.terrainMaterial = terrainMaterial;
    }

    /// <summary>
    /// Renderiza todos los cubos del terreno y los almacena en una matriz.
    /// </summary>
    public void RenderAllCubes(GameObject chunkObject, int[,,] terrainMatrix, Vector3Int chunkSize)
    {
        float spacing = 1.1f; // Espaciado entre cubos
        cubes = new GameObject[chunkSize.x, chunkSize.y, chunkSize.z];

        for (int x = 0; x < chunkSize.x; x++)
        {
            for (int y = 0; y < chunkSize.y; y++)
            {
                for (int z = 0; z < chunkSize.z; z++)
                {
                    // Calcula la posición del cubo
                    Vector3 position = new Vector3(x * spacing, z * spacing, y * spacing);

                    // Instancia el cubo en la posición calculada
                    GameObject cube = Object.Instantiate(cubePrefab, position, Quaternion.identity, chunkObject.transform);
                    cube.name = $"Cube_{x}_{y}_{z}";

                    // Configura el material y color inicial (verde)
                    Renderer renderer = cube.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        renderer.material = terrainMaterial;
                        renderer.material.color = Color.green; // Terreno base verde
                    }

                    // Almacena el cubo en la matriz
                    cubes[x, y, z] = cube;
                }
            }
        }
    }

    /// <summary>
    /// Cambia el color de un cubo existente a rojo (camino).
    /// </summary>
    public void SetCubeAsPath(Vector3Int position)
    {
        if (cubes == null || position.x < 0 || position.y < 0 || position.z < 0 ||
            position.x >= cubes.GetLength(0) || position.y >= cubes.GetLength(1) || position.z >= cubes.GetLength(2))
        {
            Debug.LogWarning("Intentando acceder a un cubo fuera de los límites.");
            return;
        }

        GameObject cube = cubes[position.x, position.y, position.z];
        if (cube != null)
        {
            Renderer renderer = cube.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = Color.red; // Cambia el color a rojo
            }
        }
    }
}
