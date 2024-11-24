using UnityEngine;

public interface IChunkGenerator
{
    void GenerateChunk(Vector3Int chunkSize);
}
