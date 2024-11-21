using UnityEngine;

public interface IPathGenerator
{
    void GeneratePath(int[,,] matrix, Vector3Int chunkSize, Vector3Int startPoint);
    bool IsOnEdge(Vector3Int point, Vector3Int chunkSize);
    Vector3Int GetNextPoint(Vector3Int currentPoint, Vector3Int chunkSize);
}

