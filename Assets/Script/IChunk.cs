using UnityEngine;

public interface IChunk
{
    void Initialize(Vector3Int size, int[,,] matrix);
    Vector3Int GetSize();
    int[,,] GetMatrix();
}
