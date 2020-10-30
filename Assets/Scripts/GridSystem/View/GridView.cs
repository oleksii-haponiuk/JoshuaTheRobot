using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
public class GridView : MonoBehaviour
{
    [SerializeField] private float tileRialSize = 1f;
    
    [SerializeField] private int chunkSizeInTiles = 32;
    
    [SerializeField] private int loadedChunksRadius = 4;

    [SerializeField] private Camera mainCamera;
    
    private Dictionary<Vector2Int, Chunk> _activeChunks = new Dictionary<Vector2Int, Chunk>();
    
    private Vector2Int _centerChunkCoordinates;

    
    private void Awake()
    {
        Grid.OnUpdateTile += HandleUpdateTile;
    }

    private void OnDestroy()
    {
        Grid.OnUpdateTile -= HandleUpdateTile;
    }

    private void Update()
    {
        var updatedCenterChunkCoordinates = RealPositionToChunkCoordinates(mainCamera.transform.position);
        if (_centerChunkCoordinates == updatedCenterChunkCoordinates)
        {
            return;
        }
        _centerChunkCoordinates = updatedCenterChunkCoordinates;
        UpdateChunks();
    }
    
    private void HandleUpdateTile(int x, int y, Tile tile)
    {
        var chunkCoordinates = TileCoordinatesToChunkCoordinates(new Vector2Int(x, y));
        var chunkOffsetCoordinates 
            = new Vector2Int(x % (chunkCoordinates.x * chunkSizeInTiles), y % (chunkCoordinates.y * chunkSizeInTiles));

        if (_activeChunks.ContainsKey(chunkCoordinates))
        {
            var oldTile = _activeChunks[chunkCoordinates].Tiles[chunkOffsetCoordinates.x, chunkOffsetCoordinates.y];
            Destroy(oldTile);
            var tileGameObject = Instantiate(tile.tileType.tilePrefab, gameObject.transform, true);
            tileGameObject.transform.position = new Vector2(x * tileRialSize, y * tileRialSize);
            _activeChunks[chunkCoordinates].Tiles[chunkOffsetCoordinates.x, chunkOffsetCoordinates.y] = tileGameObject;
        }
    }

    private void UpdateChunks()
    {
        var updatedActiveChunksCoordinates = GetCurrentActiveChunksCoordinates(_centerChunkCoordinates);
        UnloadUnusedChunks(updatedActiveChunksCoordinates);
        CreateNewChunks(updatedActiveChunksCoordinates);
    }

    private List<Vector2Int> GetCurrentActiveChunksCoordinates(Vector2Int currentCentralPosition)
    {
        var maxChunksLenght = Grid.Instance.Length / chunkSizeInTiles;
        var maxChunksWidth = Grid.Instance.Width / chunkSizeInTiles;
        
        var firstChunkX = Mathf.Clamp(currentCentralPosition.x - loadedChunksRadius, 0, maxChunksLenght); 
        var lastChunkX = Mathf.Clamp(currentCentralPosition.x + loadedChunksRadius, 0, maxChunksLenght);
        var firstChunkY = Mathf.Clamp(currentCentralPosition.y - loadedChunksRadius, 0, maxChunksWidth);
        var lastChunkY = Mathf.Clamp(currentCentralPosition.y + loadedChunksRadius, 0, maxChunksWidth);
       
        var result = new List<Vector2Int>();
        for (var x = firstChunkX; x <= lastChunkX; x++)
        {
            for (var y = firstChunkY; y <= lastChunkY; y++)
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
                    var tile = Grid.Instance.GetTile(xGridCoordinates, yGridCoordinates);
                    var tileGameObject = Instantiate(tile.tileType.tilePrefab, gameObject.transform, true);
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
        var chunksCoordinatesToUnload = _activeChunks.Keys.Except(updatedActiveChunks).ToList();
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
        return new Vector2Int((int)(realPosition.x / tileRialSize), (int)(realPosition.y / tileRialSize));
    }

    public Vector2 TileCoordinatesToRealPosition(Vector2Int tileCoordinates)
    {
        return new Vector3(tileRialSize * tileCoordinates.x, tileRialSize * tileCoordinates.y);
    }
    
    public Vector2Int RealPositionToChunkCoordinates(Vector3 realPosition)
    {
        var chunkRealSize = chunkSizeInTiles * tileRialSize;
        return new Vector2Int((int)(realPosition.x / chunkRealSize), (int)(realPosition.y / chunkRealSize));
    }

    public Vector2 ChunkCoordinatesToRealPosition(Vector2Int chunkCoordinates)
    {
        var chunkRealSize = chunkSizeInTiles * tileRialSize;
        return new Vector3(chunkRealSize * chunkCoordinates.x, chunkRealSize * chunkCoordinates.y);
    }
    
    public Vector2Int TileCoordinatesToChunkCoordinates(Vector2Int tileCoordinates)
    {
        return new Vector2Int(tileCoordinates.x / chunkSizeInTiles, tileCoordinates.y / chunkSizeInTiles);
    }
}