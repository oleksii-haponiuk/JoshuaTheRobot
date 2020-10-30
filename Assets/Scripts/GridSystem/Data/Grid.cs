using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public static event Action<int, int, Tile> OnUpdateTile;
    
    [SerializeField] private int length;
    
    [SerializeField] private int width;

    [SerializeField] private TileType defaultTileType;
    
    private Tile[,] _tiles;
    
    public int Width => width;
    
    public int Length => length;

    public static Grid Instance { get; private set; }
    
    
    private void Awake()
    {
        Instance = this;
        _tiles = new Tile[width, length];
        for (int x = 0; x < length; x++)
        {
            for (int y = 0; y < width; y++)
            {
                _tiles[x, y] = new Tile(defaultTileType);
            } 
        }
    }

    public Tile GetTile(int x, int y)
    {
        return _tiles[x, y];
    }

    public void SetTile(int x, int y, TileType tileType)
    {
        _tiles[x, y] = new Tile(tileType);
        OnUpdateTile?.Invoke(x, y, _tiles[x, y]);
    }
}
