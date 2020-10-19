using UnityEngine;
using UnityEngine.Tilemaps;

public class TileDataBase : MonoBehaviour
{
    public static TileDataBase Instance
    {
        get;
        private set;
    }

    void Awake()
    {
        Instance = this;
    }
}