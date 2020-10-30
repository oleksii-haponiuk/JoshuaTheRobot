using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
public class TerrainVisualisation : MonoBehaviour
{
    [SerializeField] private float tileRialSize = 1f;
    
    [SerializeField] private int chunkSizeInTiles = 32;
    
    [SerializeField] private int loadedChunksRadius = 4;

    [SerializeField] private Camera mainCamera;
    
    private Dictionary<Vector2Int, Chunk> _activeChunks = new Dictionary<Vector2Int, Chunk>();
    
    private Vector2Int _centerChunkCoordinates;


    private void Update()
    {
        var updatedCenterChunkCoordinates = RealPositionToChunkCoordinates(mainCamera.transform.position);
        if (_centerChunkCoordinates == updatedCenterChunkCoordinates)
        {
            return;
        }
        _centerChunkCoordinates = updatedCenterChunkCoordinates;
        UpdateChunks(_centerChunkCoordinates);
    }

    private void UpdateChunks(Vector2 cameraCenterPosition)
    {
        var updatedActiveChunksCoordinates = GetCurrentActiveChunksCoordinates(RealPositionToChunkCoordinates(cameraCenterPosition));
        UnloadUnusedChunks(updatedActiveChunksCoordinates);
        CreateNewChunks(updatedActiveChunksCoordinates);
    }

    private List<Vector2Int> GetCurrentActiveChunksCoordinates(Vector2Int currentCentralPosition)
    {
        var result = new List<Vector2Int>();
        var startXChunkPosition = 
            Mathf.Clamp(currentCentralPosition.x - loadedChunksRadius, 0, Terrain.Instance.Length % chunkSizeInTiles); 
        var endXChunkPosition = 
            Mathf.Clamp(currentCentralPosition.x + loadedChunksRadius, 0, Terrain.Instance.Length % chunkSizeInTiles);
        var startYChunkPosition = 
            Mathf.Clamp(currentCentralPosition.x - loadedChunksRadius, 0, Terrain.Instance.Width % chunkSizeInTiles);
        var endYChunkPosition = 
            Mathf.Clamp(currentCentralPosition.x - loadedChunksRadius, 0, Terrain.Instance.Width % chunkSizeInTiles);
        for (var x = startXChunkPosition; x <= endXChunkPosition; x++)
        {
            for (var y = startYChunkPosition; y <= endYChunkPosition; y++)
            {
                result.Add(new Vector2Int(x, y));
            }    
        }

        return result;
    }

    private void CreateNewChunks(IEnumerable<Vector2Int> updatedActiveChunks)
    {
        var chunksCoordinatesToCreate = updatedActiveChunks.Except(_activeChunks.Keys);
        foreach (var chunkCoordinates in chunksCoordinatesToCreate)
        {
            CreateChunk(chunkCoordinates);
        }
    }
    
    private bool CreateChunk(Vector2Int chunkCoordinates)
    {
        if (!_activeChunks.ContainsKey(chunkCoordinates))
        {
            var tilesGameObjects = new GameObject[chunkSizeInTiles, chunkSizeInTiles];
            
            var xOffset = chunkCoordinates.x * chunkSizeInTiles;
            var yOffset = chunkCoordinates.y * chunkSizeInTiles;
            
            for (var x = 0; x < chunkSizeInTiles; x++)
            {
                for (var y = 0; y < chunkSizeInTiles; y++)
                {
                    var xGridCoordinates = x + xOffset;
                    var yGridCoordinates = y + yOffset;
                    var tile = Terrain.Instance.GetTile(xGridCoordinates, yGridCoordinates);
                    var tileGameObject = Instantiate(tile.tileType.tilePrefab);
                    tileGameObject.transform.position = new Vector2(xGridCoordinates * tileRialSize, yGridCoordinates * tileRialSize);
                    tilesGameObjects[x, y] = tileGameObject;
                } 
            }
            
            _activeChunks[chunkCoordinates] = new Chunk(tilesGameObjects);
            
            return true;
        }
        return false;
    }
    
    private void UnloadUnusedChunks(IEnumerable<Vector2Int> updatedActiveChunks)
    {
        var chunksCoordinatesToUnload = _activeChunks.Keys.Except(updatedActiveChunks);
        foreach (var chunkCoordinates in chunksCoordinatesToUnload)
        {
            UnloadChunk(chunkCoordinates);
        }
    }

    private bool UnloadChunk(Vector2Int chunkCoordinates)
    {
        if (_activeChunks.ContainsKey(chunkCoordinates))
        {
            var chunk = _activeChunks[chunkCoordinates];
            foreach (var tile in chunk.Tiles)
            {
                Destroy(tile);
            }

            _activeChunks.Remove(chunkCoordinates);
            return true;
        }
        return false;
    }
    
    public Vector2Int RealPositionToTileCoordinates(Vector2 realPosition)
    {
        return new Vector2Int((int)(realPosition.x % tileRialSize), (int)(realPosition.y % tileRialSize));
    }

    public Vector2 TileCoordinatesToRealPosition(Vector2Int tileCoordinates)
    {
        return new Vector2(tileRialSize * tileCoordinates.x, tileRialSize * tileCoordinates.y);
    }
    
    public Vector2Int RealPositionToChunkCoordinates(Vector2 realPosition)
    {
        var chunkRealSize = chunkSizeInTiles * tileRialSize;
        return new Vector2Int((int)(realPosition.x % chunkRealSize), (int)(realPosition.y % chunkRealSize));
    }

    public Vector2 ChunkCoordinatesToRealPosition(Vector2Int chunkCoordinates)
    {
        var chunkRealSize = chunkSizeInTiles * tileRialSize;
        return new Vector2(chunkRealSize * chunkCoordinates.x, chunkRealSize * chunkCoordinates.y);
    }
}