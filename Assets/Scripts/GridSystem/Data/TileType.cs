using UnityEngine;

[UnityEngine.CreateAssetMenu(fileName = "TileType", menuName = "MyScriptableObject/TileType", order = 0)]
public class TileType : ScriptableObject
{
    public GameObject tilePrefab;
}