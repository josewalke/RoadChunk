using UnityEngine;

public class BlockRenderer : IBlockRenderer
{
    private readonly GameObject _cubePrefab;
    private readonly Transform _parent;

    public BlockRenderer(GameObject cubePrefab, Transform parent)
    {
        _cubePrefab = cubePrefab;
        _parent = parent;
    }

    /// <summary>
    /// Renderiza un bloque en la posición especificada y le asigna un nombre.
    /// </summary>
    /// <param name="position">Posición del bloque.</param>
    /// <param name="name">Nombre del bloque.</param>
    public void RenderBlock(Vector3 position, string name)
    {
        // Instancia el cubo en la posición especificada
        GameObject block = Object.Instantiate(_cubePrefab, position, Quaternion.identity, _parent);

        // Asigna el nombre especificado al cubo
        block.name = name;
    }
}
