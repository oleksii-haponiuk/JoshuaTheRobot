using UnityEngine;

[UnityEngine.CreateAssetMenu(fileName = "TileType", menuName = "MyScriptableObject/TileType", order = 0)]
public class TileType : UnityEngine.ScriptableObject
{
    public int id;
    public GameObject tilePrefab;
}