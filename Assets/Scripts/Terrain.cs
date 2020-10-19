using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain : MonoBehaviour
{
    [SerializeField] private int length;
    
    [SerializeField] private int width;

    [SerializeField] private TileType defaultTileType;
    
    private Tile[,] _tiles;
    
    public int Width => width;
    
    public int Length => length;

    public static Terrain Instance
    {
        get;
        private set;
    }
    void Awake()
    {
        Instance = this;
        _tiles = new Tile[width, length];
        for (int x = 0; x < length; x++)
        {
            for (int y = 0; y < width; y++)
            {
                _tiles[x, y] = new Tile(x, y, defaultTileType);
            } 
        }
    }

    void Update()
    {
        
    }

    public Tile GetTile(int x, int y)
    {
        return _tiles[x, y];
    }
}
