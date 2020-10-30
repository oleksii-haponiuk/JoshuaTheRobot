using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileDataBase : MonoBehaviour
{
    [SerializeField] private List<TileType> tileTypes;

    private Dictionary<TileType, int> _idByTileType = new Dictionary<TileType, int>();

    public ReadOnlyCollection<TileType> TileTypes => new ReadOnlyCollection<TileType>(tileTypes);
    
    public static TileDataBase Instance { get; private set; }

    
    private void Awake()
    {
        Instance = this;
        //Generating dictionary for fast access id by TileType
        for (var i = 0; i < tileTypes.Count; i++)
        {
            _idByTileType[tileTypes[i]] = i;
        }
    }

    public TileType IdToTileType(int id)
    {
        return tileTypes[id];
    }
    
    public int TileTypeToId(TileType tileType)
    {
        return _idByTileType[tileType];
    }
}